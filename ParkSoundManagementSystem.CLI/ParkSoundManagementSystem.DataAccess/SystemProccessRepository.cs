using ParkSoundManagementSystem.Core.Exceptions;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core;
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

        public async Task<bool> IsContainsString()
        {
            var str = await OpenAndRead();
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return true;
        }

        public async Task<DesiredProcess> Read()
        {
            var text = await OpenAndRead();
            var _context = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var processName = _context[0];
            if (int.TryParse(_context[1], out int pId))
            {
                return new DesiredProcess(processName, pId);
            }

            throw new FileExistException("File does not exsist");

        }

        public async Task<int> Write(DesiredProcess process)
        {
            if (IsExist)
            {
                using (StreamWriter writer = new StreamWriter(_filePath, false))
                {
                    await writer.WriteLineAsync("");
                }
            }

            string text = process.Name + " " + process.PId.ToString();
            using (var stream = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                var buffer = Encoding.Default.GetBytes(text);
                await stream.WriteAsync(buffer, 0, buffer.Length);

            }
            return process.PId;
        }

        
        private async Task<string> OpenAndRead()
        {
            string text = "";
            if (IsExist)
            {
                using (FileStream stream = File.OpenRead(_filePath))
                {
                    var buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                    text = Encoding.UTF8.GetString(buffer);
                }
                return text;
            }
            throw new FileExistException("File does not exsist");
        }


    }
}
