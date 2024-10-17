using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Repositories.AppDbContext;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(256)]
    public string Password { get; set; }
        
    // You can add more fields like Email, CreatedDate, etc.
}