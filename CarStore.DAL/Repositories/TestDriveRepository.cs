using CarStore.BO;
using Microsoft.EntityFrameworkCore;

namespace CarStore.DAL.Repositories
{
    public class TestDriveRepository : GenericRepository<TestDrive>, ITestDriveRepository
    {
        private readonly CarStoreDbContext _context;

        public TestDriveRepository(CarStoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestDrive>> GetAllTestDrivesAsync()
        {
            return await _context.TestDrives
                .Include(td => td.User)
                .Include(td => td.Car)
                .OrderByDescending(o => o.ScheduleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TestDrive>> GetTestDrivesByUserIdAsync(int userId)
        {
            return await _context.TestDrives
                .Where(td => td.UserId == userId)
                .Include(td => td.Car)
                .OrderByDescending(td => td.ScheduleDate)
                .ToListAsync();
        }

        public async Task<TestDrive?> GetTestDriveByIdAsync(int testDriveId)
        {
            return await _context.TestDrives
                .Include(td => td.User)
                .Include(td => td.Car)
                .FirstOrDefaultAsync(td => td.TestDriveId == testDriveId);
        }
    }
}
