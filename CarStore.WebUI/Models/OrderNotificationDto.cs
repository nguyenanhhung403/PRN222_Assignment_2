namespace CarStore.WebUI.Models;

/// <summary>
/// DTO for order notifications via SignalR
/// </summary>
public class OrderNotificationDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int CarId { get; set; }
    public string CarBrand { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string NotificationType { get; set; } = string.Empty; // "Created", "Updated", "StatusChanged"
    public string Message { get; set; } = string.Empty;
}
