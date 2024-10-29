using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class CourierPriority
{
    public string IdModoEnvio { get; set; } = null!;

    public string? ModoEnvío { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
