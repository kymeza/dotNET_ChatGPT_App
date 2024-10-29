namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Client
{
    public Client()
    {
        Orders = new HashSet<Order>();
    }

    public string IdCliente { get; set; } = null!;
    public string IdSegmento { get; set; } = null!;
    public string? Cliente { get; set; }

    public virtual Segment IdSegmentoNavigation { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; }
}