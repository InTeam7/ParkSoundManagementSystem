using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.DataAccess.ArgsClasses;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.DataAccess
{
    public class RepeatCountRepository : IRepeatCountRepository
    {
        private readonly RepeatCountArgs _args;
        public RepeatCountRepository(RepeatCountArgs args)
        {
            _args = args;
        }
        public bool IsExist => File.Exists(_args.FilePath);

        public async Task<int> GetRepeatCount()
        {
            if (IsExist)
            {
                using (var sr = new StreamReader(_args.FilePath))
                {
                    string text = await sr.ReadToEndAsync();
                    if (int.TryParse(text, out int count))
                    {
                        return count;
                    }
                    throw new InvalidOperationException("Text in a file not integer");
                }
            }
            else
            {
                throw new FileExistException("File does not exsist");
            }
        }

        public async Task<int> SetRepeatCount(int count)
        {
            using (var sw = new StreamWriter(_args.FilePath, false))
            {
                await sw.WriteAsync(count.ToString());
            }
            return count;
        }
    }
}
