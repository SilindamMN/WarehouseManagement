namespace Persistence
{
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Diagnostics;

  public class DataContext : DbContext
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<WareHouse> WareHouses { get; set; }
    public DbSet<ProductWareHouse> ProductWareHouses { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (optionsBuilder.IsConfigured)
      {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
      }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Configure Order relationships

      modelBuilder.Entity<ProductWareHouse>()
       .HasOne(pw => pw.Product)
       .WithMany(p => p.ProductWareHouses)
       .HasForeignKey(pw => pw.ProductId)
       .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<ProductWareHouse>()
          .HasOne(pw => pw.WareHouse)
          .WithMany(w => w.ProductWareHouses)
          .HasForeignKey(pw => pw.WareHouseId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.Product)
          .WithMany()
          .HasForeignKey(o => o.ProductId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.SourceWareHouse)
          .WithMany()
          .HasForeignKey(o => o.SourceWareHouseId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.DestinationWareHouse)
          .WithMany()
          .HasForeignKey(o => o.DestinationWareHouseId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<ProductWareHouse>()
          .HasOne(pw => pw.Product)
          .WithMany()
          .HasForeignKey(pw => pw.ProductId)
          .OnDelete(DeleteBehavior.Restrict);


      modelBuilder.Entity<ProductWareHouse>()
          .HasOne(pw => pw.WareHouse)
          .WithMany()
          .HasForeignKey(pw => pw.WareHouseId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
       .HasOne(o => o.SourceWareHouse)
       .WithMany()
       .HasForeignKey(o => o.SourceWareHouseId)
       .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.DestinationWareHouse)
          .WithMany()
          .HasForeignKey(o => o.DestinationWareHouseId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Order>()
          .HasOne(o => o.Product)
          .WithMany()
          .HasForeignKey(o => o.ProductId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Product>()
        .HasIndex(p => p.ProductCode)
        .IsUnique();

      modelBuilder.Entity<WareHouse>()
        .HasIndex(p => p.WareHouseCode)
        .IsUnique();
    }
  }
}