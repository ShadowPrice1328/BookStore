using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models.ViewModels;

namespace BookStore.Controllers;

[Route ("api/[controller]")]
[ApiController]
public class BooksController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public BooksController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/books?[...]=[...]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
        [FromQuery] string? name,
        [FromQuery] string? genre,
        [FromQuery] int? year,
        [FromQuery] int? afterYear,
        [FromQuery] int? beforeYear,
        [FromQuery] string? publishingHouse,
        [FromQuery] int available,
        [FromQuery] string? has_series,
        [FromQuery] string? series,
        [FromQuery] string? language,
        [FromQuery] double price_from,
        [FromQuery] double price_to)
    {
        IQueryable<Book> query = _bkstrContext.Books
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.IdAuthorNavigation)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.IdGenreNavigation);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(b => b.Name.StartsWith(name));
        if (!string.IsNullOrEmpty(genre))
            query = query.Where(b => b.BookGenres.Any(bg => bg.IdGenreNavigation.Name == genre));
        if (year > 0)
            query = query.Where(b => b.YearOfPublish == year);
        if (afterYear > 0)
           query = query.Where(b => b.YearOfPublish > afterYear);
        if (beforeYear > 0)
            query = query.Where(b => b.YearOfPublish < beforeYear);
        if (!string.IsNullOrEmpty(publishingHouse))
            query = query.Where(b => b.PublishingHouse == publishingHouse);
        if (available >= 0)
            query = query.Where(b => b.CountAvailable >= available);
        if (!string.IsNullOrEmpty(has_series))
        {
            if (has_series != "true" && has_series != "false")
                return BadRequest("Invalid value for has_series parameter.");

            query = has_series == "true" ? query.Where(b => b.Series != null)
                : query.Where (b => string.IsNullOrEmpty(b.Series) || b.Series == null);
        }
        if (!string.IsNullOrEmpty(series))
            query = query.Where(b => b.Series == series);
        if (!string.IsNullOrEmpty(language))
        {
            if (int.TryParse(language, out _))
                return BadRequest("Ivanid value for language parameter.");
            else query = query.Where(b => b.Language == language);
        }
        if (price_from >= 0 && price_from <= 99999999.99)
            query = query.Where(b => Convert.ToDouble(b.Price) >= price_from);
        else {return BadRequest("Invalid value for price_from parameter.");}
        if (price_to >= 0 && price_to <= 99999999.99)
            query = query.Where(b => Convert.ToDouble(b.Price) <= price_to);
        else {return BadRequest("Invalid value for price_to parameter.");}

        var books = await query.Select(b => MapBook(b)).ToListAsync();

        if (books.Count == 0)
            return NoContent();

        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var books = await _bkstrContext.Books
        .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.IdAuthorNavigation)
        .Include(b => b.BookGenres)
            .ThenInclude(bg => bg.IdGenreNavigation)
        .Select(b => MapBook(b))
        .ToListAsync();

        var book = books.FirstOrDefault(b => b.Id == id);

        return Ok(book);
    }

    private static BookViewModel MapBook(Book b)
    {
        return new BookViewModel
        {
            Id = b.Id,
            Name = b.Name,
            YearOfPublish = b.YearOfPublish,
            Price = b.Price,
            CountAvailable = b.CountAvailable,
            Description = b.Description,
            PublishingHouse = b.PublishingHouse,
            Illustrations = b.Illustrations,
            Series = b.Series,
            Isbn = b.Isbn,
            Language = b.Language,
            Translator = b.Translator,
            OriginalName = b.OriginalName,
            Pages = b.Pages,
            Picture = b.Picture,
            Authors = b.BookAuthors.Select(ba => $"{ba.IdAuthorNavigation.FirstName} {ba.IdAuthorNavigation.LastName}"),
            Genres = b.BookGenres.Select(bg => $"{bg.IdGenreNavigation.Name}")
        };
    }
}