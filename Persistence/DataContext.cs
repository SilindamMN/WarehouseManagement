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

      Seeding.Seed(modelBuilder);
      // Configure Order relationships

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
    }
  }
}