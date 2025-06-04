using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("API is alive");
    }
}

