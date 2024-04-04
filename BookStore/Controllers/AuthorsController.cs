using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/authors")]
[ApiController]
public class AuthorsController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public AuthorsController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetShipments()
    {
        return await _bkstrContext.Authors.ToListAsync();
    }
}