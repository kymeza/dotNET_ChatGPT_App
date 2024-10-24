using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class City
{
    public string IdCiudad { get; set; } = null!;

    public string IdPais { get; set; } = null!;

    public string? Ciudad { get; set; }

    public virtual Country IdPaisNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
