using System;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface ITimeService
    {
        Task<DateTime> GetTime();
        Task<DateTime> SetTime(DateTime time);
        Task<DateTime> SetDefultTime();
    }
}
