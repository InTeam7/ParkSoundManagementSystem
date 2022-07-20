using ParkSoundManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ParkSoundManagementSystem.Services
{
    public class ParkVolumeService : IParkVolumeService
    {
        private readonly IDictionary<string, IPAddress> _computers;
        public IPAddress CurrentComputer { get; set; }
        private readonly IAudioControlService _audioControlService;

        public ParkVolumeService(IAudioControlService audioControlService)
        {
            _computers = new Dictionary<string, IPAddress>()
            {
                ["Оживи у красок"] = IPAddress.Parse("192.168.1.43"),
                ["Оживи центр"] = IPAddress.Parse("192.168.1.44"),
                ["Оживи у ТТП"] = IPAddress.Parse("192.168.1.45"),
                ["VR 1"] = IPAddress.Parse("192.168.1.27"),
                ["VR 2"] = IPAddress.Parse("192.168.1.33"),
                ["VR 3"] = IPAddress.Parse("192.168.1.34"),
                ["VR 4"] = IPAddress.Parse("192.168.1.53"),
                ["Вовка трон"] = IPAddress.Parse("192.168.1.30"),
                ["Вовка кубики"] = IPAddress.Parse("192.168.1.31"),
                ["Вовка печь"] = IPAddress.Parse("192.168.1.32"),
                ["ТТП"] = IPAddress.Parse("192.168.1.29"),
                ["Петя и волк"] = IPAddress.Parse("192.168.1.51"),//192.168.1.51
                ["Карлосон"] = IPAddress.Parse("192.168.1.52"),
                ["Краски"] = IPAddress.Parse("192.168.1.38"),
                ["Фото шар"] = IPAddress.Parse("192.168.1.28"),
                ["Винни"] = IPAddress.Parse("192.168.1.47"),
                ["Кролик"] = IPAddress.Parse("192.168.1.46"),
                ["Шурале"] = IPAddress.Parse("192.168.1.37"),
                ["Голограмма Леша"] = IPAddress.Parse("192.168.1.23"),
                ["Большой телевизор"] = IPAddress.Parse("192.168.1.26"),
                ["Таксофон"] = IPAddress.Parse("192.168.1.25"),
                ["Фото герои 2экр."] = IPAddress.Parse("192.168.1.40"),
                ["Фото герои 3экр."] = IPAddress.Parse("192.168.1.41"),
                ["Голограмма телевизор"] = IPAddress.Parse("192.168.1.24"),
                ["Краски трекер"] = IPAddress.Parse("192.168.1.39"),
                ["Колодец"] = IPAddress.Parse("192.168.1.36"),
                ["Цветок"] = IPAddress.Parse("192.168.1.35"),
            };
            _audioControlService = audioControlService;
        }


        public string CreateMessage(int volume)
        {
            if (CurrentComputer.ToString() == GetMyIpAdress().ToString())
            {
                _audioControlService.SetMasterVolume(volume);
            }
            return CurrentComputer.ToString() + "_" + volume;
        }

        public List<string> GetComputers()
        {
            var computers = _computers
                .Select(x => x.Key)
                .ToList();
            return computers;
        }

        public IPAddress GetMyIpAdress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 6553);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address;
            }
        }

        public IPAddress SetComputer(string name)
        {
            if (_computers.TryGetValue(name, out IPAddress _currentComputer))
            {
                CurrentComputer = _currentComputer;
                return CurrentComputer;
            }
            throw new InvalidOperationException("Computer with this name does not exist");
        }


    }
}
