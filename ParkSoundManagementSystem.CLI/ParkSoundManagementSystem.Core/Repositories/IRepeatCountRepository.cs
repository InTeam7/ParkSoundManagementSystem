using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface IRepeatCountRepository
    {
        Task<int> SetRepeatCount(int count);
        Task<int> GetRepeatCount();
        bool IsExist { get; }
    }
}
