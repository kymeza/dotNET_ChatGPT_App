namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Segment
{
    public Segment()
    {
        Clients = new HashSet<Client>();
    }

    public string IdSegmento { get; set; } = null!;
    public string? Segmento { get; set; }

    public virtual ICollection<Client> Clients { get; set; }
}