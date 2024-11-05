using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/test")]
[Authorize]
public class TestController : ControllerBase
{
    public TestController()
    {
        
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from the test controller!");
    }
}