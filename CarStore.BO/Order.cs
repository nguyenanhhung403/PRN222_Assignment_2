using System;
using System.Collections.Generic;

namespace CarStore.BO;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
