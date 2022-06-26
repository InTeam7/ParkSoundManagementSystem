using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Services
{
    public class RepeatCountService : IRepeatCountService
    {
        private readonly IRepeatCountRepository _repeatCountRepository;
        private readonly int _defaultCount = 4;
        public int Count { get; set; }
        public RepeatCountService(IRepeatCountRepository repeatCountRepository)
        {
            _repeatCountRepository = repeatCountRepository;
            
        }

        public async Task<int> GetRepeatCount()
        {
            try
            {
                Count = await _repeatCountRepository.GetRepeatCount();
                return Count;
            }
            catch (FileExistException)
            {
                return await SetDefaultRepeatCount();
            }
            catch (InvalidOperationException)
            {
                return await SetDefaultRepeatCount();
            }
        }

        public async Task<int> SetDefaultRepeatCount()
        {
            Count = await _repeatCountRepository.SetRepeatCount(_defaultCount);
            return Count;
        }

        public async Task<int> SetRepeatCount(int count)
        {
            Count = count;
            return await _repeatCountRepository.SetRepeatCount(Count);
        }

    }
}
