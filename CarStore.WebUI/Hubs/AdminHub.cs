using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CarStore.WebUI.Hubs;

/// <summary>
/// SignalR hub for admin dashboard notifications (admin-only)
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
        await base.OnDisconnectedAsync(exception);
    }
}
