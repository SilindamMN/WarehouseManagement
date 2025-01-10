namespace UnitTest.Services
{
  using Domain.Entities;
  using Persistence;
  using System.Threading.Tasks;

  public static class MockData
  {
    public static async Task Seed(DataContext context)
    {
      // Add mock products
      context.Products.Add(new Product { Id = 1, ProductCode = "P001", ProductDescription = "Product 1", ProductQuantity = 19 });
      context.Products.Add(new Product { Id = 2, ProductCode = "P002", ProductDescription = "Product 2", ProductQuantity = 30 });

      // Add mock warehouses
      context.WareHouses.Add(new WareHouse { Id = 1, WareHouseCode = "W001", WareHouseName = "Warehouse 1" });
      context.WareHouses.Add(new WareHouse { Id = 2, WareHouseCode = "W002", WareHouseName = "Warehouse 2" });

      // Add mock product-warehouse relationships
      context.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 1, Quantity = 100 });
      context.ProductWareHouses.Add(new ProductWareHouse { ProductId = 2, WareHouseId = 2, Quantity = 50 });

      await context.SaveChangesAsync();
    }
  }
}