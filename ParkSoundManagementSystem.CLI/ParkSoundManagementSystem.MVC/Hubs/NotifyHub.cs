using Microsoft.AspNetCore.SignalR;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.MVC
{
    public class NotifyHub : Hub
    {
        private readonly ITimeService _timeService;
        private readonly IRepeatCountService _repeatCountService;
        private readonly IAudioControlService _audioControlService;
        private readonly ISystemProcessService _systemProcessService;
        private readonly ITextToSpeechService _textToSpeechService;
        private readonly IPlayAudioFileService _audioPlayService;
        private readonly IParkVolumeService _parkVolumeService;

        public NotifyHub(ITimeService timeService,
            IRepeatCountService repeatCountService,
            IAudioControlService audioControlService,
            ISystemProcessService systemProcessService,
            ITextToSpeechService textToSpeechService,
            IPlayAudioFileService audioPlayService,
            IParkVolumeService parkVolumeService)
        {
            _repeatCountService = repeatCountService;
            _timeService = timeService;
            _audioControlService = audioControlService;
            _systemProcessService = systemProcessService;
            _textToSpeechService = textToSpeechService;
            _audioPlayService = audioPlayService;
            _parkVolumeService = parkVolumeService;
        }

        public async Task SpeechText(string text)
        {
            await this.Clients.Others.SendAsync("Speech", text);
            var pId = await _systemProcessService.GetProcessId();
            _audioControlService.SetApplicationMute(pId, true);
            _textToSpeechService.Speech(text, _repeatCountService.Count);
            _audioControlService.SetApplicationMute(pId, false);
        }
        public async Task ChangeVoice(string voice)
        {
            await this.Clients.Others.SendAsync("SelectedVoice", voice);
            _textToSpeechService.SelectVoice(voice);
        }

        public async Task PlayNotify(string message)
        {
            await this.Clients.All.SendAsync("PlayNotify", message);
            var pId = await _systemProcessService.GetProcessId();
            _audioControlService.SetApplicationMute(pId, true);
            _audioPlayService.PlayNotify(_repeatCountService.Count);
            _audioControlService.SetApplicationMute(pId, false);
        }


        public async Task TurnOnSound(string message)
        {
            await this.Clients.All.SendAsync("TurnOnSound", message);
            var pId = await _systemProcessService.GetProcessId();
            _audioControlService.SetApplicationMute(pId, false);
        }

        public async Task TurnOffSound(string message)
        {
            await this.Clients.All.SendAsync("TurnOffSound", message);
            var pId = await _systemProcessService.GetProcessId();
            _audioControlService.SetApplicationMute(pId, true);
        }


        public async Task IncrementCount(int count)
        {
            if (_repeatCountService.Count < 5)
            {
                _repeatCountService.Count++;
                var _count = await _repeatCountService.SetRepeatCount(_repeatCountService.Count);
                await this.Clients.All.SendAsync("IncrementCount", _count);
            }
        }
        public async Task DecrementCount(int count)
        {
            if (_repeatCountService.Count > 1)
            {
                _repeatCountService.Count--;
                var _count = await _repeatCountService.SetRepeatCount(_repeatCountService.Count);
                await this.Clients.All.SendAsync("DecrementCount", _count);
            }
        }
        public async Task ChangeTime(string time)
        {
            var newTime = await _timeService.SetTime(DateTime.Parse(time));
            await this.Clients.Others.SendAsync("SendTime", newTime);
        }

        public async Task ChangeText(string text)
        {
            await this.Clients.All.SendAsync("SendText", text);
        }

        public async Task PlayVoiceMessage(string voiceName)
        {
            await this.Clients.All.SendAsync("PlayVoiceMessage", voiceName);
            var pId = await _systemProcessService.GetProcessId();
            _audioControlService.SetApplicationMute(pId, true);
            _audioPlayService.PlayVoiceMessage(voiceName, _repeatCountService.Count);
            _audioControlService.SetApplicationMute(pId, false);
        }
        public async Task CheckIsOnlineSelectedComputer(string computerName)
        {
            var ipAdress = _parkVolumeService.SetComputer(computerName);
            if (ipAdress.ToString() == _parkVolumeService.GetMyIpAdress().ToString())
            {
                await this.Clients.All.SendAsync("StatusResponce", _audioControlService.GetMasterVolume());
            }
            else
            {
                await this.Clients.All.SendAsync("CheckIsOnlineSelectedComputer", computerName);
            }
        }

        public async Task SetVolume(int volume)
        {
            var message = _parkVolumeService.CreateMessage(volume);
            await this.Clients.All.SendAsync("SetVolumeComputer", message);
        }
        public async Task AcceptOnlineStatusResponce(int volume)
        {
            await this.Clients.All.SendAsync("StatusResponce", volume);
        }

        public override async Task OnConnectedAsync()
        {
            var pId = await _systemProcessService.GetProcessId();
            var isMute = _audioControlService.GetApplicationMute(pId);
            if (isMute)
            {
                await this.Clients.All.SendAsync("TurnOffSound", "");
            }
            else
            {
                await this.Clients.All.SendAsync("TurnOnSound", "");
            }
            await Clients.All.SendAsync("SendCount", await _repeatCountService.GetRepeatCount());
            await Clients.All.SendAsync("SendTime", await _timeService.GetTime());
            await Clients.Caller.SendAsync("Computers", _parkVolumeService.GetComputers());
            await base.OnConnectedAsync();
        }
    }
}
