using Microsoft.AspNetCore.SignalR;
using CarStore.WebUI.Hubs;
using CarStore.BO;

namespace CarStore.WebUI.Services
{
    public interface ITestDriveNotificationService
    {
        Task NotifyTestDriveScheduledAsync(TestDrive testDrive);
        Task NotifyTestDriveUpdatedAsync(TestDrive testDrive);
        Task NotifyTestDriveCancelledAsync(int testDriveId, int userId);
    }

    public class TestDriveNotificationService : ITestDriveNotificationService
    {
        private readonly IHubContext<TestDriveHub> _hubContext;

        public TestDriveNotificationService(IHubContext<TestDriveHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTestDriveScheduledAsync(TestDrive testDrive)
        {
            // Thông báo cho admin
            await _hubContext.Clients.Group("Admins").SendAsync("TestDriveScheduled", new
            {
                TestDriveId = testDrive.TestDriveId,
                UserId = testDrive.UserId,
                CarId = testDrive.CarId,
                ScheduleDate = testDrive.ScheduleDate,
                Note = testDrive.Note,
                Status = testDrive.Status,
                Message = $"Có lịch lái thử mới được đặt cho ngày {testDrive.ScheduleDate:dd/MM/yyyy HH:mm}"
            });

            // Thông báo cho user
            await _hubContext.Clients.Group($"User_{testDrive.UserId}").SendAsync("TestDriveScheduled", new
            {
                TestDriveId = testDrive.TestDriveId,
                CarId = testDrive.CarId,
                ScheduleDate = testDrive.ScheduleDate,
                Note = testDrive.Note,
                Status = testDrive.Status,
                Message = $"Lịch lái thử của bạn đã được xác nhận cho ngày {testDrive.ScheduleDate:dd/MM/yyyy HH:mm}"
            });
        }

        public async Task NotifyTestDriveUpdatedAsync(TestDrive testDrive)
        {
            // Thông báo cho admin
            await _hubContext.Clients.Group("Admins").SendAsync("TestDriveUpdated", new
            {
                TestDriveId = testDrive.TestDriveId,
                UserId = testDrive.UserId,
                CarId = testDrive.CarId,
                ScheduleDate = testDrive.ScheduleDate,
                Note = testDrive.Note,
                Status = testDrive.Status,
                Message = $"Lịch lái thử #{testDrive.TestDriveId} đã được cập nhật"
            });

            // Thông báo cho user
            await _hubContext.Clients.Group($"User_{testDrive.UserId}").SendAsync("TestDriveUpdated", new
            {
                TestDriveId = testDrive.TestDriveId,
                CarId = testDrive.CarId,
                ScheduleDate = testDrive.ScheduleDate,
                Note = testDrive.Note,
                Status = testDrive.Status,
                Message = $"Lịch lái thử của bạn đã được cập nhật"
            });
        }

        public async Task NotifyTestDriveCancelledAsync(int testDriveId, int userId)
        {
            // Thông báo cho admin
            await _hubContext.Clients.Group("Admins").SendAsync("TestDriveCancelled", new
            {
                TestDriveId = testDriveId,
                UserId = userId,
                Message = $"Lịch lái thử #{testDriveId} đã bị hủy"
            });

            // Thông báo cho user
            await _hubContext.Clients.Group($"User_{userId}").SendAsync("TestDriveCancelled", new
            {
                TestDriveId = testDriveId,
                Message = $"Lịch lái thử #{testDriveId} của bạn đã bị hủy"
            });
        }
    }
}
