using ParkSoundManagementSystem.Core;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Services
{
    public class SystemProcessService : ISystemProcessService
    {
        private readonly ISystemProcessRepository _systemProcessRepository;
        private List<DesiredProcess> _processes;
        public SystemProcessService(ISystemProcessRepository systemProcessRepository)
        {
            _systemProcessRepository = systemProcessRepository;
            _processes = new List<DesiredProcess>();
        }

        public async Task<int> GetProcessId()
        {
            var process = await _systemProcessRepository.Read();
            return process.PId;

        }

        public async Task<string> GetProcessName()
        {
            var process = await _systemProcessRepository.Read();
            return process.Name;
        }


        public async Task<string> SetProcessAutomatically()
        {
            var isExist =  _systemProcessRepository.IsExist;
            if (!isExist)
            {
                var process = FindMostLoadedProcess();
                var id = await _systemProcessRepository.Write(process);
                return process.Name;
            }
            else
            {
                var process = await _systemProcessRepository.Read();
                return process.Name;
            }
        }


        public async Task<int> SetProcess(string processName)
        {
            WriteAllProcess();
            var process = _processes.FirstOrDefault(x => x.Name == processName);
            var pId = await _systemProcessRepository.Write(process);
            return pId;
        }


        private DesiredProcess FindMostLoadedProcess()
        {
            WriteAllProcess();
            var currentProcess = _processes.Max(x => x);
            return currentProcess;
        }
        private void WriteAllProcess()
        {
            Process[] process = Process.GetProcesses();
            foreach (Process p in process)
            {
                long memoryUse = (p.WorkingSet64 / 1024);
                _processes.Add(new DesiredProcess(p.ProcessName, p.Id, memoryUse));
            }
        }

       
    }
}
