namespace Persistence
{
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;

  public class Seeding
  {

    public static void Seed(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<WareHouse>().HasData(
          new WareHouse { Id = 1, WareHouseCode = "WH001", WareHouseName = "Warehouse 1" },
          new WareHouse { Id = 2, WareHouseCode = "WH002", WareHouseName = "Warehouse 2" },
          new WareHouse { Id = 3, WareHouseCode = "WH003", WareHouseName = "Warehouse 3" }
      );

      modelBuilder.Entity<Product>().HasData(
          new Product { Id = 1, ProductCode = "P001", ProductDescription = "Product 1", ProductQuantity = 20 },
          new Product { Id = 2, ProductCode = "P002", ProductDescription = "Product 2", ProductQuantity = 20 },
          new Product { Id = 3, ProductCode = "P003", ProductDescription = "Product 3", ProductQuantity = 20 },
          new Product { Id = 4, ProductCode = "P004", ProductDescription = "Product 4", ProductQuantity = 20 },
          new Product { Id = 5, ProductCode = "P005", ProductDescription = "Product 5", ProductQuantity = 20 },
          new Product { Id = 6, ProductCode = "P006", ProductDescription = "Product 6", ProductQuantity = 20 },
          new Product { Id = 7, ProductCode = "P007", ProductDescription = "Product 7", ProductQuantity = 20 },
          new Product { Id = 8, ProductCode = "P008", ProductDescription = "Product 8", ProductQuantity = 20 },
          new Product { Id = 9, ProductCode = "P009", ProductDescription = "Product 9", ProductQuantity = 20 }
      );

      modelBuilder.Entity<ProductWareHouse>().HasData(
       new ProductWareHouse { Id = 1, ProductId = 1, WareHouseId = 1, Quantity = 20 },
       new ProductWareHouse { Id = 2, ProductId = 2, WareHouseId = 1, Quantity = 20 },
       new ProductWareHouse { Id = 3, ProductId = 3, WareHouseId = 2, Quantity = 20 },
       new ProductWareHouse { Id = 4, ProductId = 4, WareHouseId = 2, Quantity = 20 },
       new ProductWareHouse { Id = 5, ProductId = 5, WareHouseId = 3, Quantity = 20 },
       new ProductWareHouse { Id = 6, ProductId = 6, WareHouseId = 3, Quantity = 20 },
       new ProductWareHouse { Id = 7, ProductId = 7, WareHouseId = 1, Quantity = 20 },
       new ProductWareHouse { Id = 8, ProductId = 8, WareHouseId = 2, Quantity = 20 },
       new ProductWareHouse { Id = 9, ProductId = 9, WareHouseId = 3, Quantity = 20 }
   );

    }
  }
}