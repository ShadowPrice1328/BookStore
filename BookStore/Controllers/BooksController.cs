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

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
        [FromQuery] string? name,
        [FromQuery] string? genre)
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

        var books = await query.Select(b => MapBook(b)).ToListAsync();

        if (books.Count == 0)
            return NoContent();

        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBooks(int id)
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