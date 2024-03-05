using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class OrderShipment
{
    public int Id { get; set; }

    public int IdOrder { get; set; }

    public int IdShipment { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Shipment IdShipmentNavigation { get; set; } = null!;
}
