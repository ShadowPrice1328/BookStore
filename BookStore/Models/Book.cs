using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookStore.Models;

public partial class Book
{
    public Book(string name, int yearOfPublish, decimal price, int countAvailable, string? description,
                string publishingHouse, bool illustrations, string? series, string isbn,
                string language, string? translator, string? originalName, int? pages, string? picture)
    {
        Name = name;
        YearOfPublish = yearOfPublish;
        Price = price;
        CountAvailable = countAvailable;
        Description = description;
        PublishingHouse = publishingHouse;
        Illustrations = illustrations;
        Series = series;
        Isbn = isbn;
        Language = language;
        Translator = translator;
        OriginalName = originalName;
        Pages = pages;
        Picture = picture;
    }    
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int YearOfPublish { get; set; }

    public decimal Price { get; set; }

    public int CountAvailable { get; set; }

    public string? Description { get; set; }

    public string PublishingHouse { get; set; } = null!;

    public bool Illustrations { get; set; }

    public string? Series { get; set; }

    public string Isbn { get; set; } = null!;

    public string Language { get; set; } = null!;

    public string? Translator { get; set; }

    public string? OriginalName { get; set; }

    public int? Pages { get; set; }

    public string? Picture { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set;} = new List<BookAuthor>();

    [JsonIgnore]
    public virtual ICollection<BookGenre> BookGenres { get; set;} = new List<BookGenre>();

    public virtual ICollection<OrderBook> OrderBooks { get; } = new List<OrderBook>();

    public virtual ICollection<UserBook> UserBooks { get; } = new List<UserBook>();
}
