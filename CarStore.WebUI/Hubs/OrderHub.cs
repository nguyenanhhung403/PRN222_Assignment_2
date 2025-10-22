using Microsoft.AspNetCore.SignalR;

namespace CarStore.WebUI.Hubs;

/// <summary>
/// SignalR hub for real-time order notifications
/// </summary>
public class OrderHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Add admin users to admin group
        if (Context.User?.IsInRole("Admin") == true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        }

        // Add user to their personal group
        var userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User?.IsInRole("Admin") == true)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
        }

        var userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnDisconnectedAsync(exception);
    }
}
