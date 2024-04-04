using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/books")]
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
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(bool extended = false)
    {
        var query = _bkstrContext.Books;

        if (extended)
        {
            var booksWithAuthors = await query
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.IdAuthorNavigation)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.IdGenreNavigation)
                .Select(b => new
                {
                    b.Id,
                    b.Name,
                    b.YearOfPublish,
                    b.Price,
                    b.CountAvailable,
                    b.Description,
                    b.PublishingHouse,
                    b.Illustrations,
                    b.Series,
                    b.Isbn,
                    b.Language,
                    b.Translator,
                    b.OriginalName,
                    b.Pages,
                    AuthorName = string.Join(", ", b.BookAuthors.Select(ba => $"{ba.IdAuthorNavigation.FirstName} {ba.IdAuthorNavigation.LastName}")),
                    Genres = string.Join(", ", b.BookGenres.Select(bg => $"{bg.IdGenreNavigation.Name}"))
                })
                .ToListAsync();

            return Ok(booksWithAuthors);
        }

        var books = await query.ToListAsync();
        return Ok(books);
    }
}