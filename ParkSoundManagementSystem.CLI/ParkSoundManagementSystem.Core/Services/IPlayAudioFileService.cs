namespace ParkSoundManagementSystem.Core.Services
{
    public interface IPlayAudioFileService
    {
        void PlayNotify(int count);
        void PlayVoiceMessage(string name, int count);
    }
}
