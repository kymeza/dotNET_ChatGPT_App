using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class OrderDetail
{
    public double LineaDetalle { get; set; }

    public string IdPedido { get; set; } = null!;

    public string ArticuloId { get; set; } = null!;

    public double? Cantidad { get; set; }

    public double? Descuento { get; set; }

    public double? CosteEnvío { get; set; }

    public virtual Product Articulo { get; set; } = null!;

    public virtual Order IdPedidoNavigation { get; set; } = null!;
}
