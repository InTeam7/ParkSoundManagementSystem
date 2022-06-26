using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IAudioControlService
    {
        float GetApplicationVolume(int pid);
        bool GetApplicationMute(int pid);
        void SetApplicationVolume(int pid, float level);
        void SetApplicationMute(int pid, bool mute);
        

    }
}
