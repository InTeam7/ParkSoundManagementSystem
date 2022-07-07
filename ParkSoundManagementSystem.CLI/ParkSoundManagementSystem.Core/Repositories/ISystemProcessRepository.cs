using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface ISystemProcessRepository
    {
        Task<int> Write(DesiredProcess process);
        Task<DesiredProcess> Read();
        Task<bool> IsContainsString();
        bool IsExist { get; }
    }
}
