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

  /// <summary>
  /// Unit tests for the <see cref="OrderService"/> class.
  /// These tests verify the functionality of the <see cref="TransferProductAsync"/> method,
  /// including handling invalid cases such as transferring between the same source and destination warehouses,
  /// and ensuring a successful transfer of products between different warehouses.
  /// </summary>
  public class OrderServiceTests : IDisposable
  {
    private readonly DataContext dataContext;
    private readonly OrderService orderService;

    public OrderServiceTests()
    {
      var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      this.dataContext = new DataContext(dbContextOptions);
      this.orderService = new OrderService(this.dataContext);
    }

    /// <summary>
    /// Verifies that the <see cref="TransferProductAsync"/> method returns a bad request when the source and destination warehouses are the same.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task TransferProductAsyncShouldReturnBadRequestWhenSourceAndDestinationWarehousesAreSame()
    {
      // Arrange
      var orderDto = new CreateOrderDto
      {
        ProductId = 1,
        SourceWareHouseId = 1,
        DestinationWareHouseId = 1,
        ProductQuantity = 10
      };

      // Act
      var result = await this.orderService.TransferProductAsync(orderDto);

      // Assert
      Assert.False(result.IsSucceed);
      Assert.Equal(400, result.StatusCode);
      Assert.Contains("Source and destination warehouses cannot be the same", result.Message);
    }

    /// <summary>
    /// Verifies that the <see cref="TransferProductAsync"/> method successfully transfers products between different warehouses.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task TransferProductAsyncShouldReturnSuccessWhenTransferIsSuccessful()
    {
      // Arrange
      var orderDto = new CreateOrderDto
      {
        ProductId = 1,
        SourceWareHouseId = 1,
        DestinationWareHouseId = 2,
        ProductQuantity = 1
      };

      this.dataContext.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 1, Quantity = 10 });
      this.dataContext.ProductWareHouses.Add(new ProductWareHouse { ProductId = 1, WareHouseId = 2, Quantity = 10 });
      await this.dataContext.SaveChangesAsync();

      try
      {
        // Act
        var result = await this.orderService.TransferProductAsync(orderDto);

        // Assert
        Assert.True(result.IsSucceed);
        Assert.Equal(200, result.StatusCode);
      }
      catch (Exception ex)
      {
        Assert.True(false, $"Test failed with exception: {ex.Message}");
      }
    }

    /// <summary>
    /// Cleans up by deleting the in-memory database and disposing of the <see cref="DataContext"/> after the tests are completed.
    /// </summary>
    public void Dispose()
    {
      GC.SuppressFinalize(this);

      this.dataContext.Database.EnsureDeleted();
      this.dataContext.Dispose();
    }
  }
}