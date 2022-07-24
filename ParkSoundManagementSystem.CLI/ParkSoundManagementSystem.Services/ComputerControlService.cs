using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.Services.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Services
{
    public class ComputerControlService : IComputersControlService
    {
        private readonly IParkVolumeService _parkVolumeService;
        private readonly IDictionary<string, string> _macAdresess;
        public ComputerControlService(IParkVolumeService parkVolumeService)
        {
            _parkVolumeService = parkVolumeService;
            _macAdresess = new Dictionary<string, string>()
            {
                ["192.168.1.43"] = "A8-A1-59-72-97-14",
                ["192.168.1.44"] = "A8-A1-59-72-97-40",
                ["192.168.1.45"] = "FC-34-97-C4-6A-9D",
                ["192.168.1.53"] = "A8-A1-59-72-E6-8A",
                ["192.168.1.33"] = "A8-A1-59-72-EE-93",
                ["192.168.1.34"] = "A8-A1-59-72-EE-35",
                ["192.168.1.30"] = "7C-10-C9-A1-90-45",
                ["192.168.1.31"] = "7C-10-C9-A1-91-28",
                ["192.168.1.32"] = "7C-10-C9-1D-A2-25",
                ["192.168.1.29"] = "7C-10-C9-A1-8E-C4",
                ["192.168.1.52"] = "A8-A1-59-72-97-39",
                ["192.168.1.38"] = "FC-34-97-69-BE-50",
                ["192.168.1.28"] = "7C-10-C9-1D-A1-B9",
                ["192.168.1.47"] = "A8-A1-59-72-97-3D",
                ["192.168.1.46"] = "7C-10-C9-1D-A2-32",
                ["192.168.1.37"] = "7C-10-C9-1D-A1-63",
                ["192.168.1.23"] = "7C-10-C9-1D-A2-30",
                ["192.168.1.26"] = "7C-10-C9-1D-A1-EF",
                ["192.168.1.25"] = "7C-10-C9-1D-A2-36",
                ["192.168.1.40"] = "A8-A1-59-72-EE-A2",
                ["192.168.1.41"] = "A8-A1-59-72-EE-A0",
                ["192.168.1.24"] = "7C-10-C9-1D-A2-29",
                ["192.168.1.39"] = "FC-34-97-C4-65-D4",
                ["192.168.1.36"] = "7C-10-C9-1D-A1-2B",
                ["192.168.1.35"] = "7C-10-C9-A1-8E-D5",
                ["192.168.1.160"] = "00-60-E9-2B-8A-74",
                ["192.168.1.161"] = "00-60-E9-2C-64-85",
                ["192.168.1.162"] = "00-60-E9-2A-CA-19",
                ["192.168.1.163"] = "00-60-E9-2B-8A-A1",
                ["192.168.1.164"] = "00-60-E9-2C-63-59",
                ["192.168.1.165"] = "00-60-E9-2A-CA-2F",
                ["192.168.1.166"] = "00-60-E9-2B-8A-E1",
                ["192.168.1.167"] = "00-60-E9-2B-88-9A",
                ["192.168.1.168"] = "00-60-E9-2B-8A-DA",
                ["192.168.1.169"] = "00-60-E9-2C-63-FA",
                ["192.168.1.170"] = "00-60-E9-2C-5B-54",
                ["192.168.1.171"] = "00-60-E9-2B-8A-B7",
                ["192.168.1.172"] = "00-60-E9-2C-5B-26",
                ["192.168.1.173"] = "00-60-E9-2B-89-78",
                ["192.168.1.174"] = "00-60-E9-2C-64-08",
                ["192.168.1.175"] = "00-60-E9-2C-5B-07",
                ["192.168.1.51"] = "A8-A1-59-72-96-7D",
                ["192.168.1.27"] = "A8-A1-59-72-EE-84",


            };
        }
        public void Reboot(string ip)
        {
            if (ip != null)
            {
                if (_parkVolumeService.GetMyIpAdress().ToString() == ip)
                {
                    Process.Start("shutdown.exe", "-r -f -t 0");
                }
            }
        }

        public void ShutDown(string ip)
        {
            if (ip != null)
            {
                if (_parkVolumeService.GetMyIpAdress().ToString() == ip)
                {
                    Process.Start("shutdown.exe", "-s -f -t 0");
                }
            }
        }

        public IEnumerable<string> ShutDownAll()
        {
            PowerControlAllProjectors(false);
            var _allComputer = _macAdresess.Select(x => x.Key)
                 .Take(25)
                 .ToList();
            return _allComputer;
        }

        public async Task WakeOnLan(string ip)
        {
            if (_macAdresess.TryGetValue(ip, out string mac))
            {
                await WoL.Wake(mac);
            }
        }

        public async Task<bool> WakeOnLanAll()
        {
            PowerControlAllProjectors(true);
            foreach (var mac in _macAdresess.Values)
            {
                await WoL.Wake(mac);
            }
            return true;
        }
        private void PowerControlAllProjectors(bool TurnOn)
        {
            var projectors = _macAdresess.Skip(25)
                .SkipLast(2)
                .Select(x => x.Key)
                .ToList();

            foreach (var projector in projectors)
            {
                var proj = new PJLinkHelper(projector);
                if (TurnOn)
                    proj.turnOn();
                else
                    proj.turnOff();
            }
        }
    }
}