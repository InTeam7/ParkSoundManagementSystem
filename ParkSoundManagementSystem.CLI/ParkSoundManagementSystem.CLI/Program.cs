using Microsoft.Extensions.DependencyInjection;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.DataAccess;
using ParkSoundManagementSystem.DataAccess.ArgsClasses;
using ParkSoundManagementSystem.Services;
using System;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.CLI
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
           .AddSingleton<ProcessRepositoryArgs>(_ => new ProcessRepositoryArgs { FilePath = "ProcessConfig.txt" })
           .AddSingleton<ISystemProcessRepository, SystemProccessRepository>()
           .AddSingleton<ISystemProcessService, SystemProcessService>()

           .AddSingleton<TimeRepositoryArgs>(_ => new TimeRepositoryArgs { FilePath = "TimeConfig.txt" })
           .AddSingleton<ITimeRepository, TimeRepository>()
           .AddSingleton<ITimeService, TimeService>()

           .AddSingleton<RepeatCountArgs>(_ => new RepeatCountArgs { FilePath = "CountConfig.txt" })
           .AddSingleton<IRepeatCountRepository, RepeatCountRepository>()
           .AddSingleton<IRepeatCountService, RepeatCountService>()

           .AddSingleton<IAudioControlService, AudioControlService>()
           .AddSingleton<ITextToSpeechService, TextToSpeechService>()
           .AddSingleton<IPlayAudioFileService, PlayAudioFileService>()
           .AddSingleton<IGarbageCleaningService, GarbageCleaningService>()
           .AddSingleton<IParkVolumeService, ParkVolumeService>()

           .BuildServiceProvider();

            var _systemProcessService = serviceProvider.GetService<ISystemProcessService>();
            var name = await _systemProcessService.SetProcessAutomatically();
            var pId = await _systemProcessService.GetProcessId();
            var _audioControlService = serviceProvider.GetService<AudioControlService>();
            _audioControlService.SetApplicationMute(pId, false);
            var _garbageCleaningService = serviceProvider.GetService<IGarbageCleaningService>();
            int count = _garbageCleaningService.GetCountFiles();
            if (count > 50)
            {
                _garbageCleaningService.DeleteFiles();
            }

            var hub = new ConsoleHub(serviceProvider);
            while (true)
            {
                try
                {
                    await hub.RecieveMessages();
                    break;
                }
                catch (Exception)
                {
                    await Task.Delay(5000);
                }
            }
            while (true)
            {

            }
        }
    }
}
