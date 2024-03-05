using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class OrderBook
{
    public int Id { get; set; }

    public int IdOrder { get; set; }

    public int IdBook { get; set; }

    public int Quantity { get; set; }

    public virtual Book IdBookNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
