using CarStore.BO;
using CarStore.DAL.Repositories;
using Microsoft.AspNetCore.SignalR;
using CarStore.WebUI.Hubs;
using CarStore.WebUI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarStore.BLL.Services
{
    public class TestDriveService : ITestDriveService
    {
        private readonly ITestDriveRepository _testDriveRepository;
        private readonly IHubContext<TestDriveHub> _testDriveHubContext;
        private readonly IHubContext<AdminHub> _adminHubContext;

        public TestDriveService(
            ITestDriveRepository testDriveRepository,
            IHubContext<TestDriveHub> testDriveHubContext,
            IHubContext<AdminHub> adminHubContext)
        {
            _testDriveRepository = testDriveRepository;
            _testDriveHubContext = testDriveHubContext;
            _adminHubContext = adminHubContext;
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

            // Send SignalR notification
            var notification = new TestDriveNotificationDto
            {
                TestDriveId = testDrive.TestDriveId,
                UserId = testDrive.UserId,
                UserName = testDrive.User?.Email ?? "Unknown",
                CarId = testDrive.CarId,
                CarBrand = testDrive.Car?.Brand ?? "",
                CarModel = testDrive.Car?.Model ?? "",
                ScheduleDate = testDrive.ScheduleDate,
                Status = testDrive.Status ?? "Scheduled",
                Note = testDrive.Note,
                NotificationType = "Scheduled",
                Message = $"New test drive scheduled for {testDrive.Car?.Brand} {testDrive.Car?.Model}"
            };

            // Notify admins
            await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveTestDriveNotification", notification);

            // Notify the user who scheduled the test drive
            await _testDriveHubContext.Clients.Group($"user_{testDrive.UserId}").SendAsync("ReceiveTestDriveConfirmation", notification);
        }

        public async Task UpdateTestDriveAsync(TestDrive testDrive)
        {
            _testDriveRepository.Update(testDrive);
            await _testDriveRepository.SaveAsync();

            // Send SignalR notification about test drive update
            var notification = new TestDriveNotificationDto
            {
                TestDriveId = testDrive.TestDriveId,
                UserId = testDrive.UserId,
                UserName = testDrive.User?.Email ?? "Unknown",
                CarId = testDrive.CarId,
                CarBrand = testDrive.Car?.Brand ?? "",
                CarModel = testDrive.Car?.Model ?? "",
                ScheduleDate = testDrive.ScheduleDate,
                Status = testDrive.Status ?? "Scheduled",
                Note = testDrive.Note,
                NotificationType = "Updated",
                Message = $"Test drive #{testDrive.TestDriveId} status changed to {testDrive.Status}"
            };

            // Notify admins
            await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveTestDriveUpdate", notification);

            // Notify the user whose test drive was updated
            await _testDriveHubContext.Clients.Group($"user_{testDrive.UserId}").SendAsync("ReceiveTestDriveUpdate", notification);
        }

        public async Task<IEnumerable<TestDrive>> GetTestDrivesByUserIdAsync(int userId)
        {
            return await _testDriveRepository.GetTestDrivesByUserIdAsync(userId);
        }
    }
}
