namespace CarStore.WebUI.Models;

/// <summary>
/// DTO for inventory (car stock) updates via SignalR
/// </summary>
public class InventoryUpdateDto
{
    public int CarId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int PreviousStock { get; set; }
    public int CurrentStock { get; set; }
    public int StockChange { get; set; }
    public string UpdateType { get; set; } = string.Empty; // "Sale", "Restock", "Adjustment"
    public DateTime UpdatedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}
