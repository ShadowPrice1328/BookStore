using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Order
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Description { get; set; }

    public bool CallMe { get; set; }

    public int RecipientId { get; set; }

    public bool Gift { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; } = new List<OrderBook>();

    public virtual ICollection<OrderShipment> OrderShipments { get; } = new List<OrderShipment>();

    public virtual Recipient Recipient { get; set; } = null!;
}
