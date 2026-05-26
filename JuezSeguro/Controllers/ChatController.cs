using JuezSeguro.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuezSeguro.Controllers
{
    [Authorize(Policy = "UsuarioInterno")]   // Solo roles: Juez, Auditor, Administrador, OperadorTecnico
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChatController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /Chat
        public async Task<IActionResult> Index(string sala = "general")
        {
            // Normalizar nombre de sala
            sala = sala.ToLower().Trim();
            ViewBag.Sala = sala;

            // Los últimos 50 mensajes se cargan desde el hub al conectarse (SignalR).
            // Aquí solo pasamos metadata de la sala para que la vista la use.
            return View();
        }
    }
}
