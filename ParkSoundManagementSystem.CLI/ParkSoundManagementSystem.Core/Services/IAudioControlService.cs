namespace ParkSoundManagementSystem.Core.Services
{
    public interface IAudioControlService
    {
        float GetApplicationVolume(int pid);
        bool GetApplicationMute(int pid);
        void SetApplicationVolume(int pid, float level);
        void SetApplicationMute(int pid, bool mute);
        void SetMasterVolume(int volume);
        int GetMasterVolume();



    }
}
