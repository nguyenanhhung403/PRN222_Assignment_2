using CarStore.BO;
using CarStore.WebUI.Hubs;
using CarStore.WebUI.Models;
using Microsoft.AspNetCore.SignalR;

namespace CarStore.WebUI.Services;

/// <summary>
/// Service to handle SignalR notifications
/// </summary>
public class SignalRNotificationService : ISignalRNotificationService
{
    private readonly IHubContext<OrderHub> _orderHubContext;
    private readonly IHubContext<TestDriveHub> _testDriveHubContext;
    private readonly IHubContext<InventoryHub> _inventoryHubContext;
    private readonly IHubContext<AdminHub> _adminHubContext;

    public SignalRNotificationService(
        IHubContext<OrderHub> orderHubContext,
        IHubContext<TestDriveHub> testDriveHubContext,
        IHubContext<InventoryHub> inventoryHubContext,
        IHubContext<AdminHub> adminHubContext)
    {
        _orderHubContext = orderHubContext;
        _testDriveHubContext = testDriveHubContext;
        _inventoryHubContext = inventoryHubContext;
        _adminHubContext = adminHubContext;
    }

    public async Task NotifyOrderCreated(Order order)
    {
        var notification = new OrderNotificationDto
        {
            OrderId = order.OrderId,
            UserId = order.UserId,
            UserName = order.User?.Email ?? "Unknown",
            CarId = order.CarId,
            CarBrand = order.Car?.Brand ?? "",
            CarModel = order.Car?.Model ?? "",
            Quantity = order.Quantity ?? 0,
            TotalAmount = order.TotalAmount,
            Status = order.Status ?? "Pending",
            OrderDate = order.OrderDate ?? DateTime.Now,
            NotificationType = "Created",
            Message = $"New order placed for {order.Car?.Brand} {order.Car?.Model}"
        };

        // Notify admins
        await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveOrderNotification", notification);

        // Notify the user who placed the order
        await _orderHubContext.Clients.Group($"user_{order.UserId}").SendAsync("ReceiveOrderConfirmation", notification);
    }

    public async Task NotifyOrderUpdated(Order order)
    {
        var notification = new OrderNotificationDto
        {
            OrderId = order.OrderId,
            UserId = order.UserId,
            UserName = order.User?.Email ?? "Unknown",
            CarId = order.CarId,
            CarBrand = order.Car?.Brand ?? "",
            CarModel = order.Car?.Model ?? "",
            Quantity = order.Quantity ?? 0,
            TotalAmount = order.TotalAmount,
            Status = order.Status ?? "Pending",
            OrderDate = order.OrderDate ?? DateTime.Now,
            NotificationType = "Updated",
            Message = $"Order #{order.OrderId} status changed to {order.Status}"
        };

        // Notify admins
        await _adminHubContext.Clients.Group("Admins").SendAsync("ReceiveOrderUpdate", notification);

        // Notify the user whose order was updated
        await _orderHubContext.Clients.Group($"user_{order.UserId}").SendAsync("ReceiveOrderUpdate", notification);
    }

    public async Task NotifyTestDriveScheduled(TestDrive testDrive)
    {
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

    public async Task NotifyTestDriveUpdated(TestDrive testDrive)
    {
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

    public async Task NotifyInventoryUpdated(Car car, int previousStock)
    {
        var stockChange = (car.Stock ?? 0) - previousStock;
        var updateType = stockChange > 0 ? "Restock" : "Sale";

        var notification = new InventoryUpdateDto
        {
            CarId = car.CarId,
            Brand = car.Brand,
            Model = car.Model,
            PreviousStock = previousStock,
            CurrentStock = car.Stock ?? 0,
            StockChange = stockChange,
            UpdateType = updateType,
            UpdatedAt = DateTime.Now,
            Message = $"{car.Brand} {car.Model} stock updated: {previousStock} → {car.Stock}"
        };

        // Broadcast to all clients watching inventory
        await _inventoryHubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", notification);

        // Also notify specific car watchers
        await _inventoryHubContext.Clients.Group($"car_{car.CarId}").SendAsync("ReceiveCarStockUpdate", notification);
    }
}
