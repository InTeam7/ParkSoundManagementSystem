using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.CLI
{
    public class ConsoleHub
    {
        private readonly HubConnection _hubConnection;
        private readonly ServiceProvider _collection;
        private readonly ITimeService _timeService;
        private readonly IRepeatCountService _repeatCountService;
        private readonly IAudioControlService _audioControlService;
        private readonly ISystemProcessService _systemProcessService;
        private readonly ITextToSpeechService _textToSpeechService;
        private readonly IPlayAudioFileService _audioPlayService;
        private readonly IParkVolumeService _parkVolumeService;
        public ConsoleHub(ServiceProvider collection)
        {
            _collection = collection;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://192.168.0.164:5002/send",
                options =>
                {
                    options.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // bypass SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                })
                .Build();
            _hubConnection.Closed += async (error) =>
             {
                 await Task.Delay(new Random().Next(0, 5) * 1000);
                 await _hubConnection.StartAsync();
             };
            _timeService = _collection.GetService<ITimeService>();
            _repeatCountService = _collection.GetService<IRepeatCountService>();
            _audioControlService = _collection.GetService<IAudioControlService>();
            _systemProcessService = _collection.GetService<ISystemProcessService>();
            _textToSpeechService = _collection.GetService<ITextToSpeechService>();
            _audioPlayService = _collection.GetService<IPlayAudioFileService>();
            _parkVolumeService = _collection.GetService<IParkVolumeService>();
        }

        public Task RecieveMessages()
        {
            _hubConnection.On<int>("SendCount", async z =>
            {
                var count = await _repeatCountService.SetRepeatCount(z);
            });
            _hubConnection.On<DateTime>("SendTime", z =>
            {
                _timeService.SetTime(z);
            });
            _hubConnection.On<string>("Speech", async z =>
            {
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, true);
                _textToSpeechService.Speech(z, _repeatCountService.Count);
                _audioControlService.SetApplicationMute(pId, false);
            });
            _hubConnection.On<string>("SelectedVoice", z =>
            {
                _textToSpeechService.SelectVoice(z);
            });
            _hubConnection.On<string>("PlayNotify", async z =>
            {
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, true);
                _audioPlayService.PlayNotify(_repeatCountService.Count);
                _audioControlService.SetApplicationMute(pId, false);
            });
            _hubConnection.On<string>("TurnOnSound", async z =>
            {
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, false);
            });
            _hubConnection.On<string>("TurnOffSound", async z =>
            {
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, true);
            });
            _hubConnection.On<int>("IncrementCount", async z =>
            {
                var _count = await _repeatCountService.SetRepeatCount(_repeatCountService.Count);
            });
            _hubConnection.On<int>("DecrementCount", async z =>
            {
                var _count = await _repeatCountService.SetRepeatCount(_repeatCountService.Count);
            });
            _hubConnection.On<DateTime>("ChangeTime", async z =>
            {
                var newTime = await _timeService.SetTime(z);
            });
            _hubConnection.On<string>("DownLoad", z =>
            {
                string downloadFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", z);
                string fileAdress = Path.Combine("ftp://192.168.0.194:21/UploadedFiles/", z);
                WebClient client = new WebClient();
                client.DownloadFile(
                    fileAdress, downloadFile);
            });
            _hubConnection.On<string>("PlayVoiceMessage", async z =>
            {
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, true);
                _audioPlayService.PlayVoiceMessage(z, _repeatCountService.Count);
                _audioControlService.SetApplicationMute(pId, false);
            });
            _hubConnection.On<string>("CheckIsOnlineSelectedComputer", async computerName =>
             {
                 var ipAdress = _parkVolumeService.SetComputer(computerName);
                 if (ipAdress.ToString() == _parkVolumeService.GetMyIpAdress().ToString())
                 {
                     await _hubConnection.SendAsync("AcceptOnlineStatusResponce", _audioControlService.GetMasterVolume());
                 }
             });
            _hubConnection.On<string>("SetVolumeComputer", computerName =>
            {
                var _keyWords = computerName.Split('_', StringSplitOptions.RemoveEmptyEntries);
                if (_keyWords[0] == _parkVolumeService.GetMyIpAdress().ToString())
                {
                    _audioControlService.SetMasterVolume(int.Parse(_keyWords[1]));
                }
            });

            return _hubConnection.StartAsync();

        }
    }
}
