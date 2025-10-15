using CarStore.BO;

namespace CarStore.DAL.Repositories
{
    public interface ITestDriveRepository : IGenericRepository<TestDrive>
    {
        Task<IEnumerable<TestDrive>> GetAllTestDrivesAsync();
        Task<IEnumerable<TestDrive>> GetTestDrivesByUserIdAsync(int userId);
        Task<TestDrive?> GetTestDriveByIdAsync(int testDriveId);
    }
}
