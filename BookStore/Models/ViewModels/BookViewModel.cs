namespace BookStore.Models.ViewModels;

public class BookViewModel
{
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
    public IEnumerable<string> Authors { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = null!;
}