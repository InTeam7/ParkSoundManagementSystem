using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface ISystemProcessRepository
    {
        Task<int> Write(string processName, int pId);
        Task<Tuple<string, int>> Read();
        bool IsExist { get; }
    }
}
