using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using BookStore.Models.Exceptions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly BookStoreContext _bkstrContext;
        private readonly IMemoryCache _cache;
        private IQueryable<Book> query;

        public BooksController(BookStoreContext bkstrContext, IMemoryCache cache)
        {
            _bkstrContext = bkstrContext;
            _cache = cache;

            query = _bkstrContext.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.IdAuthorNavigation)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.IdGenreNavigation);
        }

        // GET: api/books?[...]=[...]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] string? name, [FromQuery] string? genre, [FromQuery] string? year, [FromQuery] string? after_year, [FromQuery] string? before_year, [FromQuery] string? publishing_house, [FromQuery] string? available, [FromQuery] string? has_series, [FromQuery] string? series, [FromQuery] string? language, [FromQuery] double? price_from, [FromQuery] double? price_to)
        {
            var cacheKey = GetCacheKey(name, genre, year, after_year, before_year, publishing_house, available, has_series, series, language, price_from, price_to);

            if (!_cache.TryGetValue(cacheKey, out List<BookViewModel>? filteredBooks))
            {
                try
                {
                    query = ApplyFilters(query, name, genre, year, after_year, before_year, publishing_house, available, has_series, series, language, price_from, price_to);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (YearException ex)
                {
                    return BadRequest($"Invalid value for {ex.parameter} parameter. Must be a valid integer.");
                }

                var books = await query.Select(b => MapBook(b)).ToListAsync();

                if (books.Count == 0)
                    return NoContent();

                _cache.Set(cacheKey, books, TimeSpan.FromMinutes(10));
                filteredBooks = books;
            }
            return Ok(filteredBooks);
        }

        private static string GetCacheKey(params object?[] args)
        {
            var cacheKeyBuilder = new StringBuilder("books_");

            foreach (var arg in args)
            {
                if (arg != null && !string.IsNullOrEmpty(arg.ToString()))
                {
                    cacheKeyBuilder.Append(arg.ToString()).Append("_");
                }
            }

            return cacheKeyBuilder.ToString().TrimEnd('_');
        }

        private static IQueryable<Book> ApplyFilters(IQueryable<Book> query, string? name, string? genre, string? year, string? after_year, string? before_year, string? publishing_house, string? available, string? has_series, string? series, string? language, double? price_from, double? price_to)
        {
            if (!string.IsNullOrEmpty(name))
                query = query.Where(b => b.Name.StartsWith(name));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.BookGenres.Any(bg => bg.IdGenreNavigation.Name == genre));

            if (!string.IsNullOrEmpty(year))
            {
                if (!int.TryParse(year, out int yearInt) || yearInt < 0)
                    throw new YearException("Invalid value for year parameter. Must be a valid integer.", "year");
                query = query.Where(b => b.YearOfPublish == yearInt);
            }

            if (!string.IsNullOrEmpty(after_year))
            {
                if (!int.TryParse(after_year, out int afterYear) || afterYear < 0)
                    throw new YearException("Invalid value for after_year parameter. Must be a valid integer.", "after_year");
                query = query.Where(b => b.YearOfPublish > afterYear);
            }

            if (!string.IsNullOrEmpty(before_year))
            {
                if (!int.TryParse(before_year, out int beforeYear) || beforeYear <= 0)
                    throw new YearException("Invalid value for before_year parameter. Must be a valid integer.", "before_year");
                query = query.Where(b => b.YearOfPublish < beforeYear);
            }

            if (!string.IsNullOrEmpty(publishing_house))
                query = query.Where(b => b.PublishingHouse == publishing_house);

            if (!string.IsNullOrEmpty(available))
            {
                if (!int.TryParse(available, out int availableInt) || availableInt < 0)
                    throw new ArgumentException("Invalid value for available parameter. Must be a valid integer.");
                query = query.Where(b => b.CountAvailable >= availableInt);
            }

            if (!string.IsNullOrEmpty(has_series))
            {
                if (has_series != "true" && has_series != "false")
                    throw new ArgumentException("Invalid value for has_series parameter.");

                query = has_series == "true" ? query.Where(b => b.Series != null)
                    : query.Where(b => string.IsNullOrEmpty(b.Series));
            }

            if (!string.IsNullOrEmpty(series))
                query = query.Where(b => b.Series == series);

            if (!string.IsNullOrEmpty(language))
            {
                if (int.TryParse(language, out _))
                    throw new ArgumentException("Invalid value for language parameter.");
                query = query.Where(b => b.Language == language);
            }

            if (price_from != null)
            {
                if (price_from >= 0 && price_from <= 99999999.99)
                    query = query.Where(b => Convert.ToDouble(b.Price) >= price_from);
                else throw new ArgumentException("Invalid value for price_from parameter.");
            }

            if (price_to != null)
            {
                if (price_to >= 0 && price_to <= 99999999.99)
                    query = query.Where(b => Convert.ToDouble(b.Price) <= price_to);
                else throw new ArgumentException("Invalid value for price_to parameter.");
            }
            return query;
        }

        // GET: api/books/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var cacheKey = $"filteredBook_{id}";

            if (!_cache.TryGetValue(cacheKey, out BookViewModel? filteredBook))
            {
                var book = await query.FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return NotFound();

                filteredBook = MapBook(book);
                _cache.Set(cacheKey, filteredBook, TimeSpan.FromMinutes(10));
            }

            return Ok(filteredBook);
        }

        // GET: api/books/{id}/{specification}}
        [HttpGet("{id:int}/{specification:alpha}")]
        public async Task<ActionResult<Book>> GetBook(int id, [FromRoute] string specification)
        {
            var cacheKey = $"filteredBook_{id}";
            dynamic? response = null;

            if (!_cache.TryGetValue(cacheKey, out BookViewModel? filteredBook))
            {
                var book = await query.FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return NotFound();

                filteredBook = MapBook(book);
                _cache.Set(cacheKey, filteredBook, TimeSpan.FromMinutes(10));
            }

            switch (specification.ToLower())
            {
                case "name": response = filteredBook!.Name; break;
                case "year": response = filteredBook!.YearOfPublish; break;
                case "price": response = filteredBook!.Price; break;
                case "available": response = filteredBook!.CountAvailable; break;
                case "description": response = filteredBook!.Description; break;
                case "publishinghouse": response = filteredBook!.PublishingHouse; break;
                case "illustrations": response = filteredBook!.Illustrations; break;
                case "series": response = filteredBook!.Series; break;
                case "isbn": response = filteredBook!.Isbn; break;
                case "language": response = filteredBook!.Language; break;
                case "translator": response = filteredBook!.Translator; break;
                case "originalname": response = filteredBook!.OriginalName; break;
                case "page": response = filteredBook!.Pages; break;
                case "picture": response = filteredBook!.Picture; break;
                case "authors": response = filteredBook!.Authors; break;
                case "genres": response = filteredBook!.Genres; break;
                default: return BadRequest("Invalid specification parameter.");
            }

            return Ok(response);
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
                Authors = b.BookAuthors.Select(ba => $"{ba.IdAuthorNavigation.FirstName} {ba.IdAuthorNavigation.LastName}").ToList(),
                Genres = b.BookGenres.Select(bg => $"{bg.IdGenreNavigation.Name}").ToList()
            };
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromForm] BookFormData formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = new Book(formData.Name, formData.YearOfPublish, formData.Price, formData.CountAvailable,
                                formData.Description, formData.PublishingHouse, formData.Illustrations,
                                formData.Series, formData.Isbn, formData.Language, formData.Translator,
                                formData.OriginalName, formData.Pages, null);

            if (formData.Picture != null && formData.Picture.Length > 0)
            {
                var fileName = $"{book.Id}.jpg";
                var filePath = Path.Combine("Images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Picture.CopyToAsync(stream);
                }
                book.Picture = fileName;
            }

            _bkstrContext.Books.Add(book);
            await _bkstrContext.SaveChangesAsync(); // Save to get the book ID

            if (formData.Picture != null && formData.Picture.Length > 0)
            {
                var fileName = $"{book.Id}.jpg";
                var filePath = Path.Combine("Images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formData.Picture.CopyToAsync(stream);
                }
                book.Picture = fileName;

                // Update the book's picture path
                _bkstrContext.Books.Update(book);
                await _bkstrContext.SaveChangesAsync();
            }

            if (formData.Genres != null && formData.Genres.Any())
            {
                foreach (var _genre in formData.Genres)
                {
                    var genre = await _bkstrContext.Genres.FirstOrDefaultAsync(g => g.Name == _genre);
                    if (genre == null)
                    {
                        genre = new Genre { Name = _genre };
                        _bkstrContext.Genres.Add(genre);
                    }

                    await _bkstrContext.SaveChangesAsync();

                    var bookGenre = new BookGenre
                    {
                        IdBook = book.Id,
                        IdGenre = genre.Id
                    };
                    _bkstrContext.BookGenres.Add(bookGenre);
                }
            }

            await _bkstrContext.SaveChangesAsync();

            return Ok();
        }
    }
}
