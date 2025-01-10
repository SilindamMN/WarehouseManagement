namespace UnitTest.Services
{
  using Application.Services;
  using AutoMapper;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;
  using System;
  using System.Threading.Tasks;
  using Xunit;

  /// <summary>
  /// Contains unit tests for the <see cref="ProductService"/> class, verifying product-related operations.
  /// </summary>
  public class ProductServiceTests : IDisposable
  {
    private readonly DataContext dataContext;
    private readonly ProductService productService;
    private readonly IMapper mapper;
    private readonly GenericService<Product, ProductDto> genericService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductServiceTests"/> class.
    /// </summary>
    public ProductServiceTests()
    {
      var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;

      this.dataContext = new DataContext(dbContextOptions);
      this.productService = new ProductService(this.dataContext);
      this.genericService = new GenericService<Product, ProductDto>(this.dataContext, this.mapper);

      MockData.Seed(this.dataContext).Wait();
    }

    /// <summary>
    /// Verifies that the <see cref="GenericService{T, TDto}.GetAllAsync"/> method returns all products.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetAllProductsAsyncShouldReturnAllProducts()
    {
      // Act
      var result = await this.genericService.GetAllAsync();

      // Assert
      Assert.NotNull(result);
    }

    /// <summary>
    /// Verifies that the <see cref="GenericService{T, TDto}.GetAllAsync"/> method returns an empty list when no products exist.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetAllProductsAsyncShouldReturnEmptyListWhenNoProductsExist()
    {
      // Arrange
      this.dataContext.Products.RemoveRange();
      await this.dataContext.SaveChangesAsync();

      // Act
      var result = await this.genericService.GetAllAsync();

      // Assert
      Assert.Empty(result);
    }

    /// <summary>
    /// Verifies that the <see cref="ProductService.CreateProductAsync"/> method successfully creates a new product.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task CreateProductAsyncShouldReturnSuccessWhenProductIsCreated()
    {
      // Arrange
      var createProductDto = new CreateProductDto
      {
        ProductCode = "P003",
        ProductQuantity = 100,
        ProductDescription = "Product 3",
        WareHouseCode = "W001"
      };

      // Act
      var result = await this.productService.CreateProductAsync(createProductDto);

      // Assert
      Assert.True(result.IsSucceed);
      Assert.Equal(200, result.StatusCode);
      Assert.Contains("Product created and stored in Warehouse successfully", result.Message);
    }

    /// <summary>
    /// Verifies that the <see cref="ProductService.CreateProductAsync"/> method fails when the product code is not unique.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task CreateProductAsyncShouldFailWhenProductCodeIsNotUnique()
    {
      // Arrange
      var createProductDto = new CreateProductDto
      {
        ProductCode = "P001",
        ProductQuantity = 100,
        ProductDescription = "Product 1",
        WareHouseCode = "W001"
      };

      // Act
      var result = await this.productService.CreateProductAsync(createProductDto);

      // Assert
      Assert.False(result.IsSucceed);
      Assert.Equal(400, result.StatusCode);
      Assert.Contains("Product code must be unique", result.Message);
    }

    /// <summary>
    /// Verifies that the <see cref="ProductService.CreateProductAsync"/> method fails when the product data is invalid.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task CreateProductAsyncShouldFailWhenDataIsInvalid()
    {
      // Arrange
      var createProductDto = new CreateProductDto
      {
        ProductCode = "",
        ProductQuantity = 100,
        ProductDescription = "Product 1",
        WareHouseCode = "W001"
      };

      // Act
      var result = await this.productService.CreateProductAsync(createProductDto);

      // Assert
      Assert.False(result.IsSucceed);
      Assert.Equal(400, result.StatusCode);
      Assert.Contains("Product code is required", result.Message);
    }

    /// <summary>
    /// Verifies that the <see cref="ProductService.GetProductsByProductCodeOrWarehouseCodeAsync"/> method returns products when a product code is provided.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetProductsByProductCodeOrWarehouseCodeAsyncShouldReturnProductsWhenProductCodeIsProvided()
    {
      // Arrange
      var productCode = "P001";

      // Act
      var result = await this.productService.GetProductsByProductCodeOrWarehouseCodeAsync(productCode, null);

      // Assert
      Assert.Single(result);
      Assert.Equal(productCode, result[0].ProductCode);
    }

    /// <summary>
    /// Verifies that the <see cref="ProductService.GetProductsByProductCodeOrWarehouseCodeAsync"/> method returns products when a warehouse code is provided.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetProductsByProductCodeOrWarehouseCodeAsyncShouldReturnProductsWhenWarehouseCodeIsProvided()
    {
      // Arrange
      var warehouseCode = "W001";

      // Act
      var result = await this.productService.GetProductsByProductCodeOrWarehouseCodeAsync(null, warehouseCode);

      // Assert
      Assert.Single(result);
      Assert.Equal(warehouseCode, result[0].WareHouseCode);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ProductServiceTests"/> class.
    /// </summary>
    public void Dispose()
    {
      GC.SuppressFinalize(this);

      this.dataContext.Database.EnsureDeleted();
      this.dataContext.Dispose();
    }
  }
}