using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Domain.Helpers;
using Backend.Domain.Repositories.AppDbContext;
using Backend.Models;
using Backend.Models.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controller;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly AppDbContext _appDbContext;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginController(JwtSettings jwtSettings, AppDbContext appDbContext, ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtSettings;
        _appDbContext = appDbContext;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserLoginModel user)
    {
        var userIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

        _logger.LogInformation("User {user} has requested login with IP: {ip}", user.Username, userIp);
        
        
        // Validate the user credentials
        if (IsValidUser(user))
        {
            var token = GenerateJwtToken(user.Username);
            _logger.LogInformation("User {user} successfully logged in", user.Username);
            return Ok(new { token });
        }
        else
        {
            _logger.LogWarning("User {user} has failed login with IP {ip}", user.Username, userIp);
            return Unauthorized("Invalid Credentials");
        }
    }

    private bool IsValidUser(UserLoginModel user)
    {
        var userInDb = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);
        if (userInDb != null)
        {
            bool validPassword = Argon2PasswordHasher.VerifyHashedPassword(userInDb.Password, user.Password);
            return validPassword;
        }
        return false;
     }

    private string GenerateJwtToken(string username)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim("Permission", "Chat")
                // Add additional claims if needed
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            // Optionally set issuer and audience
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}