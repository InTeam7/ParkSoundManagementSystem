﻿using System.Collections.Generic;
using System.Net;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IParkVolumeService
    {
        IPAddress CurrentComputer { get; set; }
        IPAddress SetComputer(string name);
        IPAddress GetMyIpAdress();
        string CreateMessage(int volume);
        List<string> GetComputers();
    }
}
