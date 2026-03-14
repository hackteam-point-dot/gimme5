using Microsoft.AspNetCore.Mvc;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    [HttpGet]
    public IActionResult OkResult()
    {
        return Ok("ok");
    }
}
