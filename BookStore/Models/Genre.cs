using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<BookGenre> BookGenres { get; } = new List<BookGenre>();
}
