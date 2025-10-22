namespace CarStore.WebUI.Models;

/// <summary>
/// DTO for test drive notifications via SignalR
/// </summary>
public class TestDriveNotificationDto
{
    public int TestDriveId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int CarId { get; set; }
    public string CarBrand { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public DateTime ScheduleDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string NotificationType { get; set; } = string.Empty; // "Scheduled", "Updated", "StatusChanged"
    public string Message { get; set; } = string.Empty;
}
