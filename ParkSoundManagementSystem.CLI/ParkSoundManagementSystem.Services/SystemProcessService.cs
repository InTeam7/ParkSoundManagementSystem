using ParkSoundManagementSystem.Core;
using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Services
{
    public class SystemProcessService : ISystemProcessService
    {
        private readonly ISystemProcessRepository _systemProcessRepository;
        private readonly IAudioControlService _audioControlService;
        private List<DesiredProcess> _processes;
        public SystemProcessService(ISystemProcessRepository systemProcessRepository, IAudioControlService audioControlService)
        {
            _systemProcessRepository = systemProcessRepository;
            _processes = new List<DesiredProcess>();
            _audioControlService = audioControlService;

        }

        public async Task<int> GetProcessId()
        {
            try
            {
                var process = await _systemProcessRepository.Read();
                return process.PId;
            }
            catch (FileExistException)
            {
                throw new InvalidOperationException("File does not exist or has been deleted");
            }
        }

        public async Task<string> GetProcessName()
        {
            try
            {
                var process = await _systemProcessRepository.Read();
                return process.Name;
            }
            catch (Exception)
            {

                throw new InvalidOperationException("File does not exist or has been deleted");
            }
        }


        public async Task<string> SetProcessAutomatically()
        {
            var isExist = _systemProcessRepository.IsExist;
            if (!isExist)
            {
                var process = FindMostLoadedProcess();
                var id = await _systemProcessRepository.Write(process);
                return process.Name;
            }
            else
            {
                if (await _systemProcessRepository.IsContainsString())
                {
                    var process = await _systemProcessRepository.Read();
                    var diseredProcess = FindPidByName(process.Name);
                    var newProcc = await _systemProcessRepository.Write(diseredProcess);
                    return diseredProcess.Name;
                }
                else
                {
                    var process = FindMostLoadedProcess();
                    var id = await _systemProcessRepository.Write(process);
                    return process.Name;
                }
            }
        }

        public async Task<int> SetProcess(string processName)
        {
            WriteAllProcessInList();
            var process = _processes.FirstOrDefault(x => x.Name == processName);
            var pId = await _systemProcessRepository.Write(process);
            return pId;
        }


        private DesiredProcess FindMostLoadedProcess()
        {

            WriteAllProcessInList();
            var currentProcess = _processes.Max(x => x);
            return currentProcess;
        }
        private void WriteAllProcessInList()
        {
            _processes.Clear();
            Process[] process = Process.GetProcesses();
            foreach (Process p in process)
            {
                long memoryUse = (p.WorkingSet64 / 1024);
                _processes.Add(new DesiredProcess(p.ProcessName, p.Id, memoryUse));
            }
        }
        private DesiredProcess FindPidByName(string name)
        {
            var process = _processes.Find(x => x.Name == name);
            while (_processes.Find(x => x.Name == name) == null)
            {
                WriteAllProcessInList();
            }
            var desiredProcess = _processes.FirstOrDefault(x => x.Name == name);
            return desiredProcess;

        }


    }
}
