using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Exceptions
{
    public class FileExistException : Exception
    {
        public FileExistException(string message):base(message)
        {

        }
    }
}
