using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/genres")]
[ApiController]
public class GenresController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public GenresController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/genres
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
    {
        return await _bkstrContext.Genres.ToListAsync();
    }
}