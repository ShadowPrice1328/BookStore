using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class BookGenre
{
    public int Id { get; set; }

    public int IdBook { get; set; }

    public int IdGenre { get; set; }

    public virtual Book IdBookNavigation { get; set; } = null!;

    public virtual Genre IdGenreNavigation { get; set; } = null!;
}
