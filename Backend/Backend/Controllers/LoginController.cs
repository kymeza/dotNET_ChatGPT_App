using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Backend.Models.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private JwtSettings _jwtSettings;

    public LoginController(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
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
            return Unauthorized();
        }
    }

    private bool IsValidUser(UserLoginModel user)
    {
        // Replace this with your own validation logic (e.g., database check)
        return user.Username == "testuser" && user.Password == "password";
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
