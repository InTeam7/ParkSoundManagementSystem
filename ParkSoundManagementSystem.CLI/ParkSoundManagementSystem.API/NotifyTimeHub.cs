using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.API
{
    public class NotifyTimeHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync(message);
        }
    }
}
