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
        public void SetMasterVolume(int volume)
        {
            AudioManager.SetMasterVolume((float)volume);
        }

        public int GetMasterVolume()
        {
            var volume = AudioManager.GetMasterVolume();
            return (int)volume;
        }
    }
}
