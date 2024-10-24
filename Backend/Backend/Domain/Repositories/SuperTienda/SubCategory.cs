using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class SubCategory
{
    public string IdSubCategoria { get; set; } = null!;

    public string IdCategoria { get; set; } = null!;

    public string? SubCategoría { get; set; }

    public virtual Category IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
