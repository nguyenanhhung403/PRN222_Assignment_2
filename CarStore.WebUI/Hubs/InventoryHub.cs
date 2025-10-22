using Microsoft.AspNetCore.SignalR;

namespace CarStore.WebUI.Hubs;

/// <summary>
/// SignalR hub for real-time inventory (car stock) updates
/// </summary>
public class InventoryHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Join a specific car's inventory update group
    /// </summary>
    public async Task SubscribeToCarInventory(int carId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"car_{carId}");
    }

    /// <summary>
    /// Leave a specific car's inventory update group
    /// </summary>
    public async Task UnsubscribeFromCarInventory(int carId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"car_{carId}");
    }
}
