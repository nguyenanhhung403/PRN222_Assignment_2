using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CarStore.WebUI.Hubs
{
    public class TestDriveHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task JoinUserGroup()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
        }

        public async Task JoinAdminGroup()
        {
            var isAdmin = Context.User?.IsInRole("Admin") ?? false;
            if (isAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Tự động join vào group dựa trên role
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = Context.User?.IsInRole("Admin") ?? false;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }

            if (isAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
