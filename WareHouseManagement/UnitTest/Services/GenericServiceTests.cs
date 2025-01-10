namespace UnitTest.Services
{
  using Application.Services;
  using AutoMapper;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using Xunit;

  /// <summary>
  /// Unit tests for the <see cref="GenericService{WareHouse, WareHouseDto}"/> class.
  /// These tests verify the functionality of the service methods, including entity creation, 
  /// retrieval of active entities, and error handling for existing entities.
  /// </summary>
  public class GenericServiceTests : IDisposable
  {
    private readonly DataContext dataContext;
    private readonly GenericService<WareHouse, WareHouseDto> genericService;

    public GenericServiceTests()
    {
      var options = new DbContextOptionsBuilder<DataContext>()
          .UseInMemoryDatabase(databaseName: "TestDb")
          .Options;

      this.dataContext = new DataContext(options);
      var mapperConfig = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<WareHouse, WareHouseDto>().ReverseMap();
      });

      var mapper = mapperConfig.CreateMapper();
      this.genericService = new GenericService<WareHouse, WareHouseDto>(this.dataContext, mapper);
    }

    /// <summary>
    /// Verifies that the <see cref="CreateAsync"/> method returns an error when attempting to create an entity that already exists.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task CreateAsyncShouldReturnErrorWhenEntityExists()
    {
      // Arrange
      var testDto = new WareHouseDto { WareHouseCode = "WH001", WareHouseName = "Main Warehouse" };
      this.dataContext.WareHouses.Add(new WareHouse { WareHouseCode = "WH001", WareHouseName = "Main Warehouse" });
      await this.dataContext.SaveChangesAsync();

      // Act
      var result = await this.genericService.CreateAsync(testDto, e => e.WareHouseCode == "WH001");

      // Assert
      Assert.False(result.IsSucceed);
      Assert.Equal(400, result.StatusCode);
      Assert.Equal("Entity already exists", result.Message);
    }

    /// <summary>
    /// Verifies that the <see cref="CreateAsync"/> method successfully creates an entity when it does not already exist.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task CreateAsyncShouldCreateEntityWhenEntityDoesNotExist()
    {
      // Arrange
      var testDto = new WareHouseDto { WareHouseCode = "WH002", WareHouseName = "Backup Warehouse" };

      // Act
      var result = await this.genericService.CreateAsync(testDto, e => e.WareHouseCode == "WH002");

      // Assert
      Assert.True(result.IsSucceed);
      Assert.Equal(200, result.StatusCode);
      Assert.Equal("Created Successfully", result.Message);

      var createdEntity = await this.dataContext.WareHouses.FirstOrDefaultAsync(e => e.WareHouseCode == "WH002");
      Assert.NotNull(createdEntity);
    }

    /// <summary>
    /// Verifies that the <see cref="GetAllAsync"/> method returns only active warehouses.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetAllAsyncShouldReturnActiveWareHouses()
    {
      // Arrange
      this.dataContext.WareHouses.AddRange(
          new WareHouse { WareHouseCode = "WH001", WareHouseName = "Main Warehouse", IsActive = true },
          new WareHouse { WareHouseCode = "WH002", WareHouseName = "Backup Warehouse", IsActive = false },
          new WareHouse { WareHouseCode = "WH003", WareHouseName = "Overflow Warehouse", IsActive = true }
      );
      await this.dataContext.SaveChangesAsync();

      // Act
      var result = await this.genericService.GetAllAsync();

      // Assert
      Assert.Equal(2, result.Count());
      Assert.Contains(result, r => r.WareHouseCode == "WH001" && r.WareHouseName == "Main Warehouse");
      Assert.Contains(result, r => r.WareHouseCode == "WH003" && r.WareHouseName == "Overflow Warehouse");
      Assert.DoesNotContain(result, r => r.WareHouseCode == "WH002");
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