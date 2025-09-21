using Microsoft.AspNetCore.SignalR;
using backend.Models;

namespace backend.Hubs
{
    public class SubastasHub : Hub
    {
        // Este método se llama cuando alguien hace una puja
        public async Task NuevaPuja(Puja puja)
        {
            // Enviar la nueva puja a todos los clientes conectados
            await Clients.All.SendAsync("RecibirPuja", puja);
        }
    }
}
