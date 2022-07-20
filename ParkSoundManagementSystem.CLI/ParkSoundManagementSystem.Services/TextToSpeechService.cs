using ParkSoundManagementSystem.Core.Services;
using System.Globalization;
using System.Speech.Synthesis;

// private const string SpeechRegistryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\";
//internal const string CurrentUserVoices = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Speech\Voices";

namespace ParkSoundManagementSystem.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {

        private readonly SpeechSynthesizer _synth;
        public TextToSpeechService()
        {
            _synth = new SpeechSynthesizer();
            _synth.SetOutputToDefaultAudioDevice();
            _synth.Rate = 1;

        }
        public void SelectVoice(string gender)
        {
            var voices = _synth.GetInstalledVoices(new CultureInfo("ru-RU"));
            switch (gender)
            {
                case "men":
                    _synth.SelectVoice(voices[1].VoiceInfo.Name);
                    break;
                case "woman":
                    _synth.SelectVoice(voices[0].VoiceInfo.Name);
                    break;
                default:
                    _synth.SelectVoice(voices[1].VoiceInfo.Name);
                    break;
            }
        }

        public void Speech(string text, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                _synth.Speak(text);
            }

        }
    }
}