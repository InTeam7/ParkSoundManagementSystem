using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.DataAccess
{
    public class TimeRepository : ITimeRepository
    {
        private readonly TimeRepositoryArgs _args;
        public TimeRepository(TimeRepositoryArgs args)
        {
            _args = args;
        }

        public bool IsExist => File.Exists(_args.FilePath);

        public async Task<DateTime> GetTime()
        {
            if (IsExist)
            {
                using (var sr = new StreamReader(_args.FilePath))
                {
                    string time = await sr.ReadToEndAsync();
                    return DateTime.Parse(time);
                }
            }
            else
            {
                throw new FileExistException("File does not exsist");
            }
        }

        public async Task<DateTime> SetTime(DateTime time)
        {
            var text = string.Format("{0:T}", time);
            using (var sw = new StreamWriter(_args.FilePath, false))
            {
                await sw.WriteAsync(text);
            }
            return time;

        }

    }
}
