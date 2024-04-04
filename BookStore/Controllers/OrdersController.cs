using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/orders")]
[ApiController]
public class OrdersController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public OrdersController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _bkstrContext.Orders.ToListAsync();
    }
}