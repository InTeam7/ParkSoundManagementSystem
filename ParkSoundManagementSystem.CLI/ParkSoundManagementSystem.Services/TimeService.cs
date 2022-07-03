using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using System;
using System.Threading.Tasks;


namespace ParkSoundManagementSystem.Services
{
    public class TimeService : ITimeService
    {
        private readonly ITimeRepository _timerRepository;
        private readonly DateTime _defaultTime;

        public TimeService(ITimeRepository timeRepository)
        {
            _timerRepository = timeRepository;
            _defaultTime = new DateTime(2000, 01, 01, 19, 45, 00);
        }
        public async Task<DateTime> GetTime()
        {
            try
            {
                return await _timerRepository.GetTime();
            }
            catch (FileExistException)
            {
                return await SetDefultTime();
            }
        }

        public async Task<DateTime> SetDefultTime()
        {
            return await _timerRepository.SetTime(_defaultTime);
        }

        public async Task<DateTime> SetTime(DateTime time)
        {
            return await _timerRepository.SetTime(time);
        }
    }
}
