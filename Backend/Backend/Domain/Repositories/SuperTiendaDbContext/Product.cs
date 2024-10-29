namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class Product
{
    public Product()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public string IdArticulo { get; set; } = null!;
    public string IdSubCategoria { get; set; } = null!;
    public string? Producto { get; set; }
    public double? PrecioUnitario { get; set; }
    public double? CostoUnitario { get; set; }

    public virtual SubCategory IdSubCategoriaNavigation { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}