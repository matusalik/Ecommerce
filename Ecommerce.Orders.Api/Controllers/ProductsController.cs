using Microsoft.AspNetCore.Mvc;
using Ecommerce.Orders.Api.Data;
using Ecommerce.Orders.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Orders.Api.Entities.DTO;
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

    /*------------GET--------------*/

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound($"Product with id {id} doesn't exist.");
        }
        var productDTO = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.OrderProducts.Sum(op => op.Quantity)
        };
        return Ok(productDTO);
    }

    /*------------POST--------------*/

    [HttpPost]
    public async Task<ActionResult<Product>> Crate([FromBody] Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    /*------------DELETE--------------*/

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products
            .Include (o => o.OrderProducts)
            .FirstOrDefaultAsync (p => p.Id == id);
        if(product == null)
        {
            return NotFound($"Product with id: {id} not found.");
        }
        _context.OrderProducts.RemoveRange(product.OrderProducts);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return Ok($"Product with id: {id} successfully removed.");
    }

    /*------------PATCH--------------*/

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduct(int id, [FromBody] ProductDTO dto)
    {
        var product = await _context.Products.FindAsync(id);
        if(product == null)
        {
            return NotFound();
        }
        if(dto.Name is not null)
        {
            product.Name = dto.Name;
        }
        if(dto.Price is not null)
        {
            product.Price = dto.Price.Value;
        }
        return Ok(product);
    }
}
