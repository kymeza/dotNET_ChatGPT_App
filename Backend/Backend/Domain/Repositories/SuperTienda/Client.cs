using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class Client
{
    public string IdCliente { get; set; } = null!;

    public string IdSegmento { get; set; } = null!;

    public string? Cliente { get; set; }

    public virtual Segment IdSegmentoNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
