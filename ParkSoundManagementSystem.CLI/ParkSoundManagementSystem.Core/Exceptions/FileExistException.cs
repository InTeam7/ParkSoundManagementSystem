using System;

namespace ParkSoundManagementSystem.Core.Exceptions
{
    public class FileExistException : Exception
    {
        public FileExistException(string message) : base(message)
        {

        }
    }
}
