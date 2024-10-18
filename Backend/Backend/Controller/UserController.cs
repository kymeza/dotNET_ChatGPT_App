using Backend.Domain.Helpers;
using Backend.Domain.Repositories.AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        user.Password = Argon2PasswordHasher.HashPassword(user.Password);
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
        
        return Ok(user);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Get([FromRoute] string username)
    {
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound($"User with username '{username}' not found.");
        }
        
        return Ok(user);
    }
    
}