using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Repositories.SuperTiendaDbContext;

public partial class SuperTiendaContext : DbContext
{
    public SuperTiendaContext()
    {
    }

    public SuperTiendaContext(DbContextOptions<SuperTiendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<City> Cities { get; set; } = null!;
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Country> Countries { get; set; } = null!;
    public virtual DbSet<CourierPriority> CourierPriorities { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<Segment> Segments { get; set; } = null!;
    public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategoria)
                .HasName("PK_Categorias");

            entity.Property(e => e.IdCategoria)
                .HasMaxLength(64)
                .HasColumnName("ID Categoria");

            entity.Property(e => e.Categoría).HasMaxLength(255);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.IdCiudad)
                .HasName("PK_Ciudades");

            entity.Property(e => e.IdCiudad)
                .HasMaxLength(64)
                .HasColumnName("ID Ciudad");

            entity.Property(e => e.Ciudad).HasMaxLength(255);

            entity.Property(e => e.IdPais)
                .HasMaxLength(64)
                .HasColumnName("ID Pais");

            entity.HasOne(d => d.IdPaisNavigation)
                .WithMany(p => p.Cities)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudades_Paises");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdCliente)
                .HasName("PK_Clientes");

            entity.ToTable("Client");

            entity.Property(e => e.IdCliente)
                .HasMaxLength(64)
                .HasColumnName("ID Cliente");

            entity.Property(e => e.Cliente).HasMaxLength(255);

            entity.Property(e => e.IdSegmento)
                .HasMaxLength(64)
                .HasColumnName("ID Segmento");

            entity.HasOne(d => d.IdSegmentoNavigation)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.IdSegmento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clientes_Segmentos");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdPais)
                .HasName("PK_Paises");

            entity.Property(e => e.IdPais)
                .HasMaxLength(64)
                .HasColumnName("ID Pais");

            entity.Property(e => e.Pais).HasMaxLength(255);
        });

        modelBuilder.Entity<CourierPriority>(entity =>
        {
            entity.HasKey(e => e.IdModoEnvio)
                .HasName("PK_Modos Envio");

            entity.ToTable("Courier Priority");

            entity.Property(e => e.IdModoEnvio)
                .HasMaxLength(64)
                .HasColumnName("ID Modo Envio");

            entity.Property(e => e.ModoEnvío)
                .HasMaxLength(255)
                .HasColumnName("Modo Envío");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdPedido)
                .HasName("PK_Pedidos");

            entity.Property(e => e.IdPedido)
                .HasMaxLength(64)
                .HasColumnName("ID Pedido");

            entity.Property(e => e.ClienteId)
                .HasMaxLength(64)
                .HasColumnName("Cliente ID");

            entity.Property(e => e.FechaEnvio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha Envío");

            entity.Property(e => e.FechaPedido)
                .HasColumnType("datetime")
                .HasColumnName("Fecha Pedido");

            entity.Property(e => e.IdCiudad)
                .HasMaxLength(64)
                .HasColumnName("ID Ciudad");

            entity.Property(e => e.IdPrioridad)
                .HasMaxLength(64)
                .HasColumnName("ID Prioridad");

            entity.HasOne(d => d.Cliente)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Clientes");

            entity.HasOne(d => d.IdCiudadNavigation)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Ciudades");

            entity.HasOne(d => d.IdPrioridadNavigation)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdPrioridad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Modos Envio");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.LineaDetalle, e.IdPedido })
                .HasName("PK_Pedidos Detalles");

            entity.ToTable("Order Details");

            entity.Property(e => e.LineaDetalle).HasColumnName("Linea Detalle");

            entity.Property(e => e.IdPedido)
                .HasMaxLength(64)
                .HasColumnName("ID Pedido");

            entity.Property(e => e.ArticuloId)
                .HasMaxLength(64)
                .HasColumnName("Articulo ID");

            entity.Property(e => e.CosteEnvio).HasColumnName("Coste envío");

            entity.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos Detalles_Articulos");

            entity.HasOne(d => d.IdPedido1)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos Detalles_Pedidos");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdArticulo)
                .HasName("PK_Articulos");

            entity.Property(e => e.IdArticulo)
                .HasMaxLength(64)
                .HasColumnName("ID Articulo");

            entity.Property(e => e.IdSubCategoria)
                .HasMaxLength(64)
                .HasColumnName("ID Sub-Categoria");

            entity.Property(e => e.Producto).HasMaxLength(255);

            entity.HasOne(d => d.IdSubCategoriaNavigation)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSubCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Articulos_Sub-Categorias");
        });

        modelBuilder.Entity<Segment>(entity =>
        {
            entity.HasKey(e => e.IdSegmento)
                .HasName("PK_Segmentos");

            entity.Property(e => e.IdSegmento)
                .HasMaxLength(64)
                .HasColumnName("ID Segmento");

            entity.Property(e => e.Segmento).HasMaxLength(255);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.IdSubCategoria)
                .HasName("PK_Sub-Categorias");

            entity.ToTable("Sub-Categories");

            entity.Property(e => e.IdSubCategoria)
                .HasMaxLength(64)
                .HasColumnName("ID Sub-Categoria");

            entity.Property(e => e.IdCategoria)
                .HasMaxLength(64)
                .HasColumnName("ID Categoria");

            entity.Property(e => e.SubCategoría)
                .HasMaxLength(255)
                .HasColumnName("Sub-Categoría");

            entity.HasOne(d => d.IdCategoriaNavigation)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sub-Categorias_Categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}