using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

[Route ("api/shipments")]
[ApiController]
public class ShipmentsController : Controller
{
    private readonly BookStoreContext _bkstrContext;
    public ShipmentsController(BookStoreContext bkstrContext)
    {
        _bkstrContext = bkstrContext;
    }

    // GET: api/shipments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
    {
        return await _bkstrContext.Shipments.ToListAsync();
    }
}