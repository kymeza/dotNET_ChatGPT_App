namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public string IdPedido { get; set; } = null!;
    public string ClienteId { get; set; } = null!;
    public DateTime? FechaPedido { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public string IdPrioridad { get; set; } = null!;
    public string IdCiudad { get; set; } = null!;

    public virtual Client Cliente { get; set; } = null!;
    public virtual City IdCiudadNavigation { get; set; } = null!;
    public virtual CourierPriority IdPrioridadNavigation { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}