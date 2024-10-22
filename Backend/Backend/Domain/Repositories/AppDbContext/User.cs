using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Repositories.AppDbContext;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(64)]
    public string Username { get; set; }

    [Required]
    [MaxLength(256)]
    public string Password { get; set; }

    [MaxLength(256)]
    public string? Email { get; set; }

    [MaxLength(Int16.MaxValue)]
    public string? AboutMe { get; set; }
}
