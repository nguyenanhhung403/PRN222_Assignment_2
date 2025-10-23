using CarStore.BO;
using CarStore.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class TestDriveService : ITestDriveService
    {
        private readonly ITestDriveRepository _testDriveRepository;

        public TestDriveService(ITestDriveRepository testDriveRepository)
        {
            _testDriveRepository = testDriveRepository;
        }

        public async Task<IEnumerable<TestDrive>> GetAllTestDrivesAsync()
        {
            return await _testDriveRepository.GetAllTestDrivesAsync();
        }

        public async Task<TestDrive?> GetTestDriveByIdAsync(int testDriveId)
        {
            return await _testDriveRepository.GetTestDriveByIdAsync(testDriveId);
        }

        public async Task ScheduleTestDriveAsync(TestDrive testDrive)
        {
            // Add business logic for scheduling a test drive
            await _testDriveRepository.InsertAsync(testDrive);
            await _testDriveRepository.SaveAsync();
        }

        public async Task UpdateTestDriveAsync(TestDrive testDrive)
        {
            _testDriveRepository.Update(testDrive);
            await _testDriveRepository.SaveAsync();
        }

        public async Task<IEnumerable<TestDrive>> GetTestDrivesByUserIdAsync(int userId)
        {
            return await _testDriveRepository.GetTestDrivesByUserIdAsync(userId);
        }
    }
}
