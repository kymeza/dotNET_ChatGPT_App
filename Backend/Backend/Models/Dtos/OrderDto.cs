namespace Backend.Models.Dtos;

public class OrderDto
{
    public string IdPedido { get; set; } = null!;

    public string ClienteId { get; set; } = null!;

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaEnvío { get; set; }

    public string IdPrioridad { get; set; } = null!;

    public string IdCiudad { get; set; } = null!;
}