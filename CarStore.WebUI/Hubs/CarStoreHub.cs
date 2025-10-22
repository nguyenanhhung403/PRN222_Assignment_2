using Microsoft.AspNetCore.SignalR;

namespace CarStore.WebUI.Hubs;

/// <summary>
/// Main SignalR hub for general car store notifications
/// </summary>
public class CarStoreHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Send a notification to a specific user
    /// </summary>
    public async Task SendNotificationToUser(string userId, string message)
    {
        await Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", message);
    }

    /// <summary>
    /// Broadcast a notification to all connected clients
    /// </summary>
    public async Task BroadcastNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
