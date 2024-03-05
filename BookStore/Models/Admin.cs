using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Admin
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
