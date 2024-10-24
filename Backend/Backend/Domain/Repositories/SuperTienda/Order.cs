using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class Order
{
    public string IdPedido { get; set; } = null!;

    public string ClienteId { get; set; } = null!;

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaEnvío { get; set; }

    public string IdPrioridad { get; set; } = null!;

    public string IdCiudad { get; set; } = null!;

    public virtual Client Cliente { get; set; } = null!;

    public virtual City IdCiudadNavigation { get; set; } = null!;

    public virtual CourierPriority IdPrioridadNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
