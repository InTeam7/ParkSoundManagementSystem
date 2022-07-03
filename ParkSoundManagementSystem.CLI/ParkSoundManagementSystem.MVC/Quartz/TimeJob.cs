using Microsoft.AspNetCore.SignalR;
using ParkSoundManagementSystem.Core.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.MVC.Quartz
{
    public class TimeJob : IJob
    {
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly ITimeService _timeService;
        private readonly IPlayAudioFileService _audioPlayService;
        private readonly IRepeatCountService _repeatCountService;
        private readonly IAudioControlService _audioControlService;
        private readonly ISystemProcessService _systemProcessService;
        public TimeJob(IHubContext<NotifyHub> hubContext,
            ITimeService timeService,
            IRepeatCountService repeatCountService,
            IPlayAudioFileService audioPlayService,
            IAudioControlService audioControlService,
            ISystemProcessService systemProcessService
            )
        {
            _hubContext = hubContext;
            _timeService = timeService;
            _repeatCountService = repeatCountService;
            _audioPlayService = audioPlayService;
            _audioControlService = audioControlService;
            _systemProcessService = systemProcessService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var time = await _timeService.GetTime();
            var longTime = time.ToLongTimeString();

            if (longTime == DateTime.Now.ToLongTimeString())
            {
                await _hubContext.Clients.All.SendAsync("PlayNotifyTimer", "play");
                var pId = await _systemProcessService.GetProcessId();
                _audioControlService.SetApplicationMute(pId, true);
                _audioPlayService.PlayNotify(_repeatCountService.Count);
                _audioControlService.SetApplicationMute(pId, false);
            }


        }
    }
}
