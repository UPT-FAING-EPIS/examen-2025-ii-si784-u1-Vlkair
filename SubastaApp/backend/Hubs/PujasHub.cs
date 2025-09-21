using Microsoft.AspNetCore.SignalR;
using backend.Models;

namespace backend.Hubs
{
    public class PujasHub : Hub
    {
        // Opcional: método para enviar puja desde cliente al servidor
        // public async Task EnviarPuja(Puja puja)
        // {
        //     await Clients.All.SendAsync("RecibirPuja", puja);
        // }
    }
}