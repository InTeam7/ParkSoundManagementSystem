using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface ISystemProcessService
    {
        int GetProcessId();
        string GetProcessName();
        void SetProcess(string processName);
    }
}
