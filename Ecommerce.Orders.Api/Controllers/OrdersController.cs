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
    [HttpGet]
    [Route("findAllOrders")]
    public async Task<ActionResult<IEnumerable<Order>>> getAll()
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
                Price = op.Product.Price
            }).ToList()
        });

        return Ok(orderDtos);
    }
    [HttpPost]
    [Route("addOrder")]
    public ActionResult<Order> create(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(getAll), new {id = order.Id}, order);
    }
}
