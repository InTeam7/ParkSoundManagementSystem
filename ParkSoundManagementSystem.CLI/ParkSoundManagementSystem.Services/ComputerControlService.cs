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
                ["192.168.1.27"] = "A8-A1-59-72-EE-84",
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
                ["projector kraski 1"] = "00-60-E9-2B-8A-74",
                ["projector kraski 2"] = "00-60-E9-2C-64-85",
                ["projector kraski 3"] = "00-60-E9-2A-CA-19",
                ["projector kraski 4"] = "00-60-E9-2B-8A-A1",
                ["projector kraski 5"] = "00-60-E9-2C-63-59",
                ["projector kraski 6"] = "00-60-E9-2A-CA-2F",
                ["projector kraski 7"] = "00-60-E9-2B-8A-E1",
                ["projector kraski 8"] = "00-60-E9-2B-88-9A",
                ["projector kraski 9"] = "00-60-E9-2B-8A-DA",
                ["projector kraski 10"] = "00-60-E9-2C-63-FA",
                ["projector kraski 11"] = "00-60-E9-2C-5B-54",
                ["projector kraski 12"] = "00-60-E9-2B-8A-B7",
                ["projector kraski 13"] = "00-60-E9-2C-5B-26",
                ["projector kraski 14"] = "00-60-E9-2B-89-78",
                ["projector kraski 15"] = "00-60-E9-2C-64-08",
                ["projector kraski 16"] = "00-60-E9-2C-5B-07",
                ["192.168.1.51"] = "A8-A1-59-72-96-7D",
                ["192.168.1.53"] = "A8-A1-59-72-E6-8A",

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
            var _allComputer = _macAdresess.Select(x => x.Key)
                 .Take(24)
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
            foreach (var mac in _macAdresess.Values)
            {
                await WoL.Wake(mac);
            }
            return true;
        }
    }
}