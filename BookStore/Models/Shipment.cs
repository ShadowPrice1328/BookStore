using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Shipment
{
    public int Id { get; set; }

    public bool Finished { get; set; }

    public virtual ICollection<OrderShipment> OrderShipments { get; } = new List<OrderShipment>();
}
