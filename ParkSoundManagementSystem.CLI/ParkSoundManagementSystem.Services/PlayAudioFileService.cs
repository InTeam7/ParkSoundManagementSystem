
using NAudio.Wave;
using NetCoreAudio;
using ParkSoundManagementSystem.Core.Services;
using System.IO;
using System.Threading;

namespace ParkSoundManagementSystem.Services
{
    public class PlayAudioFileService : IPlayAudioFileService
    {
        private readonly string _fileName;
        private readonly Player _player;
        public PlayAudioFileService()
        {
            _fileName = "Mute.mp3";
            _player = new Player();
        }
        public void PlayNotify(int count)
        {

            for (int i = 0; i < count; i++)
            {

                using (var audioFile = new AudioFileReader(_fileName))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void PlayVoiceMessage(string name, int count)
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", name);
            for (int i = 0; i < count; i++)
            {
                using (var audioFile = new AudioFileReader(fileName))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }

        }
    }
}
