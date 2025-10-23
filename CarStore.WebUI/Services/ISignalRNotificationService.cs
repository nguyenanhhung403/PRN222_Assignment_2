using CarStore.BO;

namespace CarStore.WebUI.Services;

/// <summary>
/// Interface for SignalR notification service
/// </summary>
public interface ISignalRNotificationService
{
    Task NotifyOrderCreated(Order order);
    Task NotifyOrderUpdated(Order order);
    Task NotifyTestDriveScheduled(TestDrive testDrive);
    Task NotifyTestDriveUpdated(TestDrive testDrive);
    Task NotifyInventoryUpdated(Car car, int previousStock);
}
