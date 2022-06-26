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
        private readonly ProcessRepositoryArgs _args;

        public SystemProccessRepository(ProcessRepositoryArgs args)
        {
            _args = args;
        }

        public bool IsExist => File.Exists(_args.FilePath);

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
            var _keyWords = text.Split('_', StringSplitOptions.RemoveEmptyEntries);
            var processName = _keyWords[0];
            if (int.TryParse(_keyWords[1], out int pId))
            {
                return new DesiredProcess(processName, pId);
            }

            throw new FileExistException("File does not exsist");

        }

        public async Task<int> Write(DesiredProcess process)
        {
            string text = process.Name + "_" + process.PId.ToString();
            using (StreamWriter writer = new StreamWriter(_args.FilePath, false))
            {
                await writer.WriteAsync(text);
            }

            return process.PId;
        }


        private async Task<string> OpenAndRead()
        {
            if (IsExist)
            {
                using (StreamReader sr = new StreamReader(_args.FilePath))
                {
                    string text = await sr.ReadToEndAsync();
                    return text;
                }
            }
            throw new FileExistException("File does not exsist");
        }


    }
}
