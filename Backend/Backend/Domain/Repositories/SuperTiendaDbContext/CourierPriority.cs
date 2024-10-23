namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class CourierPriority
{
    public CourierPriority()
    {
        Orders = new HashSet<Order>();
    }

    public string IdModoEnvio { get; set; } = null!;
    public string? ModoEnvío { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}