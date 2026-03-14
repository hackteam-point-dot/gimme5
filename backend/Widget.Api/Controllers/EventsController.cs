using Microsoft.AspNetCore.Mvc;
using Widget.Api.ApiModels;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    [HttpPost]
    public IActionResult PostEvent([FromBody] PostEventApiModel args)
    {
        return Ok(args);
    }
}
