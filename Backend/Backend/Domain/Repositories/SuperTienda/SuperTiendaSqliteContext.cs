using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class SuperTiendaSqliteContext : DbContext
{
    public SuperTiendaSqliteContext()
    {
    }

    public SuperTiendaSqliteContext(DbContextOptions<SuperTiendaSqliteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CourierPriority> CourierPriorities { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Segment> Segments { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.IdCategoria)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Categoria");
            entity.Property(e => e.Categoría)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.IdCiudad);

            entity.Property(e => e.IdCiudad)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Ciudad");
            entity.Property(e => e.Ciudad)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");
            entity.Property(e => e.IdPais)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Pais");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("Client");

            entity.Property(e => e.IdCliente)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Cliente");
            entity.Property(e => e.Cliente)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");
            entity.Property(e => e.IdSegmento)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Segmento");

            entity.HasOne(d => d.IdSegmentoNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.IdSegmento)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdPais);

            entity.Property(e => e.IdPais)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Pais");
            entity.Property(e => e.Pais)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");
        });

        modelBuilder.Entity<CourierPriority>(entity =>
        {
            entity.HasKey(e => e.IdModoEnvio);

            entity.ToTable("Courier Priority");

            entity.Property(e => e.IdModoEnvio)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Modo Envio");
            entity.Property(e => e.ModoEnvío)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)")
                .HasColumnName("Modo Envío");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdPedido);

            entity.Property(e => e.IdPedido)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Pedido");
            entity.Property(e => e.ClienteId)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("Cliente ID");
            entity.Property(e => e.FechaEnvío)
                .HasColumnType("datetime")
                .HasColumnName("Fecha Envío");
            entity.Property(e => e.FechaPedido)
                .HasColumnType("datetime")
                .HasColumnName("Fecha Pedido");
            entity.Property(e => e.IdCiudad)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Ciudad");
            entity.Property(e => e.IdPrioridad)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Prioridad");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPrioridadNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdPrioridad)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.LineaDetalle, e.IdPedido });

            entity.ToTable("Order Details");

            entity.Property(e => e.LineaDetalle)
                .HasColumnType("float")
                .HasColumnName("Linea Detalle");
            entity.Property(e => e.IdPedido)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Pedido");
            entity.Property(e => e.ArticuloId)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("Articulo ID");
            entity.Property(e => e.Cantidad).HasColumnType("float");
            entity.Property(e => e.CosteEnvío)
                .HasColumnType("float")
                .HasColumnName("Coste envío");
            entity.Property(e => e.Descuento).HasColumnType("float");

            entity.HasOne(d => d.Articulo).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdArticulo);

            entity.Property(e => e.IdArticulo)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Articulo");
            entity.Property(e => e.CostoUnitario).HasColumnType("float");
            entity.Property(e => e.IdSubCategoria)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Sub-Categoria");
            entity.Property(e => e.PrecioUnitario).HasColumnType("float");
            entity.Property(e => e.Producto)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");

            entity.HasOne(d => d.IdSubCategoriaNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSubCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Segment>(entity =>
        {
            entity.HasKey(e => e.IdSegmento);

            entity.Property(e => e.IdSegmento)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Segmento");
            entity.Property(e => e.Segmento)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.IdSubCategoria);

            entity.ToTable("Sub-Categories");

            entity.Property(e => e.IdSubCategoria)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Sub-Categoria");
            entity.Property(e => e.IdCategoria)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(64)")
                .HasColumnName("ID Categoria");
            entity.Property(e => e.SubCategoría)
                .UseCollation("NOCASE")
                .HasColumnType("nvarchar(255)")
                .HasColumnName("Sub-Categoría");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
