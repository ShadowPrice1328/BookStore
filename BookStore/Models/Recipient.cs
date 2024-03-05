using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Recipient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
