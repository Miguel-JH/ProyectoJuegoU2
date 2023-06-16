using Microsoft.AspNetCore.SignalR;
using TopoLocoApi.Models;

namespace TopoLocoApi.Services
{
    public class TopoLocoHub : Hub
    {
        public async void Ganador(string jugador, int puntos)
        {
            await Clients.Others.SendAsync("Ganador", jugador, puntos);
        }
    }
}
