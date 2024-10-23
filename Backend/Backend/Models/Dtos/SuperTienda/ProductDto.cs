namespace Backend.Models.Dtos.SuperTienda;

public class ProductDto
{
    public string IdArticulo { get; set; } = null!;
    public string IdSubCategoria { get; set; } = null!;
    public string? Producto { get; set; }
    public double? PrecioUnitario { get; set; }
    public double? CostoUnitario { get; set; }

}