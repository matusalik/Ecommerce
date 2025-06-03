using Microsoft.AspNetCore.Mvc;
using Ecommerce.Orders.Api.Data;
using Ecommerce.Orders.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Orders.Api.Entities.DTO;
namespace Ecommerce.Orders.Api.Controllers;
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;
    public OrdersController(OrdersDbContext context)
    {
        _context = context;
    }

    /*------------GET--------------*/

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderProducts)
               .ThenInclude(op => op.Product)
            .ToListAsync();
        var orderDtos = orders.Select(order => new OrderDTO
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CustomerSurname = order.CustomerSurname,
            CreatedDate = order.CreatedDate,
            Status = order.Status.ToString(),
            Products = order.OrderProducts.Select(op => new ProductDTO
            {
                Id = op.Product.Id,
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
                
            }).ToList()
        });
        return Ok(orderDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetByIdAsync(int id)
    {
        var order = await _context.Orders
        .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
        .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return NotFound($"Order with id {id} doesn't exist.");
        }
        var orderDto = new OrderDTO
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CustomerSurname = order.CustomerSurname,
            CreatedDate = order.CreatedDate,
            Status = order.Status,
            Products = order.OrderProducts.Select(op => new ProductDTO
            {
                Id = op.Product.Id,
                Name = op.Product.Name,
                Price = op.Product.Price,
                Quantity = op.Quantity
            }).ToList()
        };
        return Ok(orderDto);
    }

    /*------------POST--------------*/

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllAsync), new {id = order.Id}, order);
    }

    [HttpPost("addProductToOrder/{oId}")]
    public async Task<IActionResult> AddProductToOrder(int oId, [FromBody] AddProductToOrderDTO dto)
    {
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.Id == oId);
        if (order == null)
        {
            return NotFound($"Order with id {oId} not found.");
        }
        var product = await _context.Products.FindAsync(dto.ProductId);
        if(product == null)
        {
            return NotFound($"Product with id {oId} not found.");
        }
        var orderProduct = new OrderProduct
        {
            OrderId = oId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };
        _context.Add(orderProduct);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /*------------DELETE--------------*/

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.Id == id);
        if(order == null)
        {
            return NotFound($"Order with id: {id} not found.");
        }
        _context.OrderProducts.RemoveRange(order.OrderProducts);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return Ok($"Order with id: {id} successfully removed.");
    }

    [HttpDelete("removeProductFromOrder/{oId}/{pId}")]
    public async Task<IActionResult> RemoveProductFormOrder(int oId, int pId)
    {
        var orderProduct = await _context.Set<OrderProduct>()
            .FirstOrDefaultAsync(op => op.OrderId == oId && op.ProductId == pId);
        if(orderProduct == null)
        {
            return NotFound($"Product with id {pId} is not a part of order with id {oId}");
        }
        _context.Remove(orderProduct);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /*------------PATCH--------------*/

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchOrder(int id, [FromBody] UpdateOrderPatchDTO dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        if (dto.CustomerName is not null)
        {
            order.CustomerName = dto.CustomerName;
        }
        if (dto.CustomerSurname is not null)
        {
            order.CustomerSurname = dto.CustomerSurname;
        }
        if (dto.Status is not null)
        {
            order.Status = dto.Status;
        }
        await _context.SaveChangesAsync();
        return Ok(order);
    }
}
