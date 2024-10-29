namespace Backend.Models.Dtos.SuperTienda;

public class OrderDetailDto
{
    public double LineaDetalle { get; set; }
    public string IdPedido { get; set; } = null!;
    public string ArticuloId { get; set; } = null!;
    public double? Cantidad { get; set; }
    public double? Descuento { get; set; }
    public double? CosteEnvio { get; set; }
}