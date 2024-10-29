namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Country
{
    public Country()
    {
        Cities = new HashSet<City>();
    }

    public string IdPais { get; set; } = null!;
    public string? Pais { get; set; }

    public virtual ICollection<City> Cities { get; set; }
}