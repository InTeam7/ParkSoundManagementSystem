namespace ParkSoundManagementSystem.Core.Services
{
    public interface IGarbageCleaningService
    {
        int GetCountFiles();
        void DeleteFiles();
    }
}
