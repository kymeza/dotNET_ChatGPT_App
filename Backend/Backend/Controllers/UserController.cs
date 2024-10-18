using Backend.Domain.Helpers;
using Backend.Domain.Repositories.AppDbContext;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _appDbContext;

    public UserController(ILogger<UserController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    // Ensure only authenticated users can create new users
    [Authorize(Roles = "Admin")]  // Only users with "Admin" role can access this endpoint
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        
        // Input validation
        if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
        {
            _logger.LogWarning("CreateUser: Invalid user data provided.");
            return BadRequest("Invalid user data.");
        }
        
        // Check for duplicate username or email
        var existingUser = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username);

        if (existingUser != null)
        {
            _logger.LogWarning("CreateUser: User with the same username or email already exists.");
            return Conflict("A user with the same username or email already exists.");
        }
        
        try
        {
            // Encrypt sensitive data (e.g., hash password)
            user.Password = Argon2PasswordHasher.HashPassword(user.Password);

            // Add user to the database
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            _logger.LogInformation("CreateUser: User {Username} successfully created.", user.Username);

            // Return user without sensitive information like password
            return Ok(new
            {
                user.Id,
                user.Username,
                CreatedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            // Log the error for debugging and audit purposes
            _logger.LogError(ex, "CreateUser: An error occurred while creating user {Username}.", user.Username);
            return StatusCode(400, "An error occurred while creating the user.");
        }
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser([FromRoute] string username)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username cannot be empty.");
        }

        if (username.Length > 50)
        {
            return BadRequest("Username exceeds the maximum length of 50 characters.");
        }

        // Assuming _dbContext is your database context and is injected via the constructor
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return NotFound($"User with username '{username}' not found.");
        }

        // If the user is found, return the user data (you might want to exclude sensitive fields like password)
        var userDto = new
        {
            user.Id,
            user.Username,
            user.Email,
            user.AboutMe
            // Add other non-sensitive fields as necessary
        };

        return Ok(userDto);
    }

    
}