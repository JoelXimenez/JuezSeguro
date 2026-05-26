using JuezSeguro.Data;
using JuezSeguro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JuezSeguro.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private const int HistorialMaximo = 50;   // mensajes recientes al conectar
        private readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }

        // ── Conexión: enviar historial solo al cliente que se conecta ─────────
        public override async Task OnConnectedAsync()
        {
            var sala = ObtenerSala();

            var historial = await _db.MensajesChat
                .Where(m => m.Sala == sala)
                .OrderByDescending(m => m.Fecha)
                .Take(HistorialMaximo)
                .OrderBy(m => m.Fecha)          // orden cronológico para mostrar
                .Select(m => new
                {
                    usuario = m.PseudonimoEmisor,
                    mensaje = m.Texto,
                    hora = m.Fecha.ToString("HH:mm")
                })
                .ToListAsync();

            // Enviar historial solo al cliente recién conectado
            foreach (var item in historial)
            {
                await Clients.Caller.SendAsync("RecibirMensaje", item.usuario, item.mensaje, item.hora);
            }

            await base.OnConnectedAsync();
        }

        // ── Envío de mensaje: persistir + broadcast ───────────────────────────
        public async Task EnviarMensaje(string mensaje)
        {
            // Sanear entrada
            if (string.IsNullOrWhiteSpace(mensaje)) return;
            if (mensaje.Length > 1000)
                mensaje = mensaje[..1000];

            var userId = Context.UserIdentifier
                           ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                           ?? "desconocido";

            var pseudo = Context.User?.Identity?.Name ?? "Anónimo";
            var sala = ObtenerSala();
            var hora = DateTime.UtcNow;

            // Persistir en BD
            var registro = new MensajeChat
            {
                UsuarioId = userId,
                PseudonimoEmisor = pseudo,
                Texto = mensaje,
                Fecha = hora,
                Sala = sala
            };

            _db.MensajesChat.Add(registro);
            await _db.SaveChangesAsync();

            // Broadcast a todos los clientes de la sala
            await Clients.All.SendAsync(
                "RecibirMensaje",
                pseudo,
                mensaje,
                hora.ToString("HH:mm"));
        }

        // ── Helper: sala basada en query-string ?sala=xxx (default "general") ─
        private string ObtenerSala()
        {
            var sala = Context.GetHttpContext()?.Request.Query["sala"].ToString();
            return string.IsNullOrWhiteSpace(sala) ? "general" : sala.ToLower().Trim();
        }
    }
}
