using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/recipients")]
[ApiController]
public class RecipientsController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public RecipientsController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/recipients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipient>>> GetRecipient()
    {
        return await _bkstrContext.Recipients.ToListAsync();
    }
}