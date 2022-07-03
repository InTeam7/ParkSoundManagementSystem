using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface ISystemProcessService
    {
        Task<int> GetProcessId();
        Task<string> GetProcessName();
        Task<int> SetProcess(string processName);
        Task<string> SetProcessAutomatically();
    }
}
