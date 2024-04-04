using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<BookAuthor> BookAuthors { get; } = new List<BookAuthor>();
}
