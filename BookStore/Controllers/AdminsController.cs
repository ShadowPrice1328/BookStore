using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/admins")]
[ApiController]
public class AdminsController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public AdminsController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/admins
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
    {
        return await _bkstrContext.Admins.ToListAsync();
    }
}