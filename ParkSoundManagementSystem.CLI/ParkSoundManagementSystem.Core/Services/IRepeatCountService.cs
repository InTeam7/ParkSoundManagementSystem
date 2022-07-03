using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Services
{
    public interface IRepeatCountService
    {
        public int Count { get; set; }
        Task<int> GetRepeatCount();
        Task<int> SetRepeatCount(int count);
        Task<int> SetDefaultRepeatCount();
    }
}
