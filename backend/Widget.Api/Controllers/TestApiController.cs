using Microsoft.AspNetCore.Mvc;
using Widget.Api.Repositories;

namespace Widget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly TestRepository _testRepository;

    public TestController(TestRepository testRepository)
    {
        _testRepository = testRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Parameter 'name' is required.");
        }

        var document = await _testRepository.CreateAsync(name, cancellationToken);
        return Ok(document);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var documents = await _testRepository.GetAllAsync(cancellationToken);
        return Ok(documents);
    }
}
