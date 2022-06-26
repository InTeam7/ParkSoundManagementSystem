using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.Services.Helpers;

namespace ParkSoundManagementSystem.Services
{
    public class AudioControlService : IAudioControlService
    {
        public bool GetApplicationMute(int pid)
        {
            if (AudioManager.GetApplicationMute(pid) == null || AudioManager.GetApplicationMute(pid) == true)
            {
                return true;
            }
            return false;
        }

        public float GetApplicationVolume(int pid)
        {
            if (AudioManager.GetApplicationVolume(pid) == null)
            {
                return 0f;
            }
            return AudioManager.GetApplicationVolume(pid).Value;
        }

        public void SetApplicationMute(int pid, bool mute)
        {
            AudioManager.SetApplicationMute(pid, mute);
        }

        public void SetApplicationVolume(int pid, float level)
        {
            AudioManager.SetApplicationVolume(pid, level);
        }
    }
}
