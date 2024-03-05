using System;
using System.Collections.Generic;

namespace BookStore.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public int YearOfPublish { get; set; }

    public decimal Price { get; set; }

    public int CountAvailable { get; set; }

    public string? Description { get; set; }

    public string PublishingHouse { get; set; } = null!;

    public bool Illustrations { get; set; }

    public string? Series { get; set; }

    public string Period { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Translator { get; set; }

    public string? OriginalName { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; } = new List<OrderBook>();

    public virtual ICollection<UserBook> UserBooks { get; } = new List<UserBook>();
}
