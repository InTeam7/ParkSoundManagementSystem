using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IRepeatCountService
    {
        public int Count { get; set; }
        Task<int> GetRepeatCount();
        Task<int> SetRepeatCount(int count);
        Task<int> SetDefaultRepeatCount();
    }
}
