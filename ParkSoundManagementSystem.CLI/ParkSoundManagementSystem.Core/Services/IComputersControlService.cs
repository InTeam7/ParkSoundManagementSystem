using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IComputersControlService
    {
        Task<bool> WakeOnLanAll();
        IEnumerable<string> ShutDownAll();
        Task WakeOnLan(string ip);
        void ShutDown(string ip);
        void Reboot(string ip);
    }
}
