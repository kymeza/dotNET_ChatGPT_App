namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Category
{
    public Category()
    {
        SubCategories = new HashSet<SubCategory>();
    }

    public string IdCategoria { get; set; } = null!;
    public string? Categoría { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; }
}