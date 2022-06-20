using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public  interface ITimeService
    {
        Task<DateTime> GetTime();
        Task<DateTime> SetTime(DateTime time);
        Task<DateTime> SetDefultTime();
    }
}
