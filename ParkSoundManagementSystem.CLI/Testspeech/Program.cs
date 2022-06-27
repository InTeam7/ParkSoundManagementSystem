using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Testspeech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = 1;
            var voices = synth.GetInstalledVoices(new CultureInfo("ru-RU"));
            synth.SelectVoice(voices[1].VoiceInfo.Name);
            //synth.SelectVoice("Microsoft Zira Desktop");

            synth.Speak("Уважаемые гости, найден телефон, подойдите на кассу");
        }
    }
}
