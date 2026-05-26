using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JuezSeguro.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task EnviarMensaje(string usuario, string mensaje)
        {
            var hora = DateTime.UtcNow.ToString("HH:mm");
            await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje, hora);
        }
    }
}
