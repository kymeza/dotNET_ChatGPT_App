using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class Product
{
    public string IdArticulo { get; set; } = null!;

    public string IdSubCategoria { get; set; } = null!;

    public string? Producto { get; set; }

    public double? PrecioUnitario { get; set; }

    public double? CostoUnitario { get; set; }

    public virtual SubCategory IdSubCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
