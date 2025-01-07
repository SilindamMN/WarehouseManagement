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

  public class OrderServiceTests : IDisposable
  {
    private readonly DataContext context;
    private readonly OrderService orderService;

    public OrderServiceTests()
    {
      var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
          .UseInMemoryDatabase(databaseName: "TestDb")
          .Options;

      context = new DataContext(dbContextOptions);
      orderService = new OrderService(context);
    }

    [Fact]
    public async Task TransferProductAsync_ShouldReturnBadRequest_WhenSourceAndDestinationWarehousesAreSame()
    {
      // Arrange
      var orderDto = new OrderDto
      {
        ProductId = 1,
        SourceWareHouseId = 1,
        DestinationWareHouseId = 1,
        ProductQuantity = 10
      };

      // Act
      var result = await orderService.TransferProductAsync(orderDto);

      // Assert
      Assert.False(result.IsSucceed);
      Assert.Equal(400, result.StatusCode);
      Assert.Contains("Source and destination warehouses cannot be the same", result.Message);
    }

    [Fact]
    public async Task TransferProductAsync_ShouldReturnSuccess_WhenTransferIsSuccessful()
    {
      // Arrange
      var orderDto = new OrderDto
      {
        ProductId = 1,
        SourceWareHouseId = 1,
        DestinationWareHouseId = 2,
        ProductQuantity = 10
      };

      // Create product warehouses in memory
      context.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 1, Quantity = 50 });
      context.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 2, Quantity = 10 });
      await context.SaveChangesAsync();

      try
      {
        // Act
        var result = await orderService.TransferProductAsync(orderDto);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.Equal(200, result.StatusCode);
        Assert.Contains("Product transferred successfully", result.Message);
      }
      catch (Exception ex)
      {
        // Log the exception message
        Assert.True(false, $"Test failed with exception: {ex.Message}");
      }
    }

    public void Dispose()
    {
      context.Database.EnsureDeleted();
      context.Dispose();
    }
  }
}