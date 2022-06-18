using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Services
{
    public class SystemProcessService : ISystemProcessService
    {
        private readonly ISystemProcessRepository _systemProcessRepository;
        public SystemProcessService(ISystemProcessRepository systemProcessRepository)
        {
            _systemProcessRepository = systemProcessRepository;
        }
        public int GetProcessId()
        {
            throw new NotImplementedException();
        }

        public string GetProcessName()
        {
            throw new NotImplementedException();
        }

        public void SetProcess(string processName)
        {
            throw new NotImplementedException();
        }
    }
}
