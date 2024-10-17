using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Repositories.AppDbContext;

public interface IAppDbContext
{
    public DbSet<User> Users { get; set; }
}