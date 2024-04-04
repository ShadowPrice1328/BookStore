using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class BookAuthor
{
    public int Id { get; set; }

    public int IdBook { get; set; }

    public int IdAuthor { get; set; }

    public virtual Author IdAuthorNavigation { get; set; } = null!;

    public virtual Book IdBookNavigation { get; set; } = null!;
}
