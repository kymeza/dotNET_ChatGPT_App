using Backend.Domain.Repositories.AppDbContext;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    
    public UserController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
        
        return Ok(user);
    }
    
}