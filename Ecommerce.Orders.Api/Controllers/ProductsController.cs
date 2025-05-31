using Microsoft.AspNetCore.Mvc;
using Ecommerce.Orders.Api.Data;
using Ecommerce.Orders.Api.Entities;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Orders.Api.Controllers;
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly OrdersDbContext _context;
    public ProductsController(OrdersDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    [Route("findAllProducts")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
}
