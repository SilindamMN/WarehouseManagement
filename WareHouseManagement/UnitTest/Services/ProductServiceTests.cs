namespace UnitTest.Services
{
  using Application.Services;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;
  using System;
  using System.Threading.Tasks;
  using Xunit;

  public class ProductServiceTests : IDisposable
  {
    private readonly DataContext context;
    private readonly ProductService productService;

    public ProductServiceTests()
    {
      var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
          .UseInMemoryDatabase(databaseName: "TestDb")
          .Options;

      context = new DataContext(dbContextOptions);
      productService = new ProductService(context);
    }

    [Fact]
    public async Task GetProductsByProductCodeOrWarehouseCodeAsync_ShouldReturnProducts_WhenProductCodeIsProvided()
    {
      // Arrange
      var productCode = "P001";
      context.Products.Add(new Product { Id = 1, ProductCode = productCode, ProductDescription = "Product 1", ProductQuantity = 19 });
      context.WareHouses.Add(new WareHouse { Id = 1, WareHouseCode = "W001", WareHouseName = "Warehouse 1" });
      context.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 1, Quantity = 100 });
      await context.SaveChangesAsync();

      // Act
      var result = await productService.GetProductsByProductCodeOrWarehouseCodeAsync(productCode, null);

      // Assert
      Assert.Single(result);
      Assert.Equal(productCode, result[0].ProductCode);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldReturnSuccess_WhenProductIsCreated()
    {
      // Arrange
      var createProductDto = new CreateProductDto
      {
        ProductCode = "P001",
        ProductQuantity = 100,
        ProductDescription = "Product 1",
        WareHouseCode = "W001"
      };

      context.WareHouses.Add(new WareHouse { Id = 1, WareHouseCode = "W001", WareHouseName = "Warehouse 1" });
      await context.SaveChangesAsync();

      // Act
      var result = await productService.CreateProductAsync(createProductDto);

      // Assert
      Assert.True(result.IsSucceed);
      Assert.Equal(200, result.StatusCode);
      Assert.Contains("Product created and stored in Warehouse successfully", result.Message);
    }

    public void Dispose()
    {
      context.Database.EnsureDeleted();
      context.Dispose();
    }
  }
}
