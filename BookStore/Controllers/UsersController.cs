using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/users")]
[ApiController]
public class UsersController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public UsersController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _bkstrContext.Users.ToListAsync();
    }
}