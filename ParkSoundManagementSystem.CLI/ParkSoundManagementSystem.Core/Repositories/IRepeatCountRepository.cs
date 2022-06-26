using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface IRepeatCountRepository
    {
        Task<int> SetRepeatCount(int count);
        Task<int> GetRepeatCount();
        bool IsExist { get; }
    }
}
