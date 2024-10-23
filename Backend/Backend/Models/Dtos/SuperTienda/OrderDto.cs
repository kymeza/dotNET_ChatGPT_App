namespace Backend.Models.Dtos.SuperTienda;

public class OrderDto
{
    public string IdPedido { get; set; } = null!;
    public string ClienteId { get; set; } = null!;
    public DateTime? FechaPedido { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public string IdPrioridad { get; set; } = null!;
    public string IdCiudad { get; set; } = null!;
}