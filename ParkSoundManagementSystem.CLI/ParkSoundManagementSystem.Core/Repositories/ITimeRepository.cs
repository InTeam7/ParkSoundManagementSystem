using System;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface ITimeRepository
    {
        Task<DateTime> SetTime(DateTime time);
        Task<DateTime> GetTime();
        bool IsExist { get; }
    }
}
