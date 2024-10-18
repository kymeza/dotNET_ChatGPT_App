using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    private readonly AppDbContext _appDbContext;
    private readonly JwtSettings _jwtSettings;

    public LoginController(JwtSettings jwtSettings, AppDbContext appDbContext)
    {
        _jwtSettings = jwtSettings;
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserLoginModel user)
    {
        // Validate the user credentials
        if (IsValidUser(user))
        {
            var token = GenerateJwtToken(user.Username);
            return Ok(new { token });
        }
        else
        {
            return Unauthorized("Invalid Credentials");
        }
    }

    private bool IsValidUser(UserLoginModel user)
    {
        var userInDb = _appDbContext.Users.FirstOrDefault(u => u.Username == user.Username);
        if (userInDb != null)
        {
            bool validPassword = userInDb.Password == user.Password;
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