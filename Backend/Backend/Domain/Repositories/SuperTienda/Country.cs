using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class Country
{
    public string IdPais { get; set; } = null!;

    public string? Pais { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
