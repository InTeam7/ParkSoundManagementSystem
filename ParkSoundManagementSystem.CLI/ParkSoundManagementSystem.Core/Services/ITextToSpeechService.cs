using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface ITextToSpeechService
    {
        void SelectVoice(string gender);
        void Speech(string text, int repeatCount);
    }
}
