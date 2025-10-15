using System;
using System.Collections.Generic;

namespace CarStore.BO;

public partial class TestDrive
{
    public int TestDriveId { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public DateTime ScheduleDate { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
