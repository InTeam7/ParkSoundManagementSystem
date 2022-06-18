using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.DataAccess
{
    public class SystemProccessRepository : ISystemProcessRepository
    {
        private readonly string _filePath;

        public SystemProccessRepository(string filePath)
        {
            _filePath = filePath;
        }

        public bool IsExist => File.Exists(_filePath);

        public async Task<Tuple<string, int>> Read()
        {
            string text = "";
            if (File.Exists(_filePath))
            {
                using (var sr = new StreamReader(_filePath))
                {
                    text = await sr.ReadLineAsync();
                }
                var tuple = text.Split(' ');
                if (int.TryParse(tuple[1], out int pId))
                {
                    return new Tuple<string, int>(tuple[0], pId);
                }
            }
            throw new FileExistException("File does not exsist");

        }

        public Task<int> Write(string processName, int pId)
        {
            throw new NotImplementedException();
        }
    }
}
