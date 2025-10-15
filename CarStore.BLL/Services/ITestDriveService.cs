using CarStore.BO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public interface ITestDriveService
    {
        Task<IEnumerable<TestDrive>> GetAllTestDrivesAsync();
        Task<TestDrive?> GetTestDriveByIdAsync(int testDriveId);
        Task ScheduleTestDriveAsync(TestDrive testDrive);
        Task UpdateTestDriveAsync(TestDrive testDrive);
        Task<IEnumerable<TestDrive>> GetTestDrivesByUserIdAsync(int userId);
    }
}
