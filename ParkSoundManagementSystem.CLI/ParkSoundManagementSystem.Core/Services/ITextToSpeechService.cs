namespace ParkSoundManagementSystem.Core.Services
{
    public interface ITextToSpeechService
    {
        void SelectVoice(string gender);
        void Speech(string text, int repeatCount);
    }
}
