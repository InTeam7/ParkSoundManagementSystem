using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
