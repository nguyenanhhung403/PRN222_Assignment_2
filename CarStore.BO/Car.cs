using System;
using System.Collections.Generic;

namespace CarStore.BO;

public partial class Car
{
    public int CarId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public decimal Price { get; set; }

    public int Year { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int? Stock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TestDrive> TestDrives { get; set; } = new List<TestDrive>();
}
