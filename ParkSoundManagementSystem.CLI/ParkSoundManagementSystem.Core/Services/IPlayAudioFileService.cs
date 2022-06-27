using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IPlayAudioFileService
    {
        void PlayNotify(int count);
        void PlayVoiceMessage(string name,int count);
    }
}
