using ParkSoundManagementSystem.Core.Services;
using System.IO;
using System.Linq;

namespace ParkSoundManagementSystem.Services
{
    public class GarbageCleaningService : IGarbageCleaningService
    {
        private readonly string _directoryPath;
        private readonly DirectoryInfo _dir;
        public GarbageCleaningService()
        {
            _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
            _dir = new DirectoryInfo(_directoryPath);
        }
        public void DeleteFiles()
        {
            foreach (var file in _dir.GetFiles())
            {
                file.Delete();
            }
        }

        public int GetCountFiles()
        {
            return _dir.GetFiles().Count();
        }
    }
}
