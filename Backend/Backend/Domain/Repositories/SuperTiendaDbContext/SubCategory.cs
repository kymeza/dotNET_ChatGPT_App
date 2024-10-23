namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class SubCategory
{
    public SubCategory()
    {
        Products = new HashSet<Product>();
    }

    public string IdSubCategoria { get; set; } = null!;
    public string IdCategoria { get; set; } = null!;
    public string? SubCategoría { get; set; }

    public virtual Category IdCategoriaNavigation { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; }
}