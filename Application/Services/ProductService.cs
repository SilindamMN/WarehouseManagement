namespace Application.Services
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;

  /// <summary>
  /// Service for managing product-related operations such as creating products and retrieving product details by product or warehouse codes.
  /// </summary>
  public class ProductService : IProductService
  {
    private readonly DataContext dataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context for interacting with the database.</param>
    public ProductService(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }

    /// <summary>
    /// Retrieves products by their product code or warehouse code.
    /// </summary>
    /// <param name="productCode">The product code to filter products by (optional).</param>
    /// <param name="warehouseCode">The warehouse code to filter products by (optional).</param>
    /// <returns>A list of `ProductWareHouseDto` containing product and warehouse details.</returns>
    public async Task<List<ProductWareHouseDto>> GetProductsByProductCodeOrWarehouseCodeAsync(string? productCode, string? warehouseCode)
    {
      var query = this.dataContext.ProductWareHouses.AsQueryable();

      if (!string.IsNullOrEmpty(productCode))
      {
        query = query.Where(pw => pw.Product.ProductCode == productCode);
      }

      if (!string.IsNullOrEmpty(warehouseCode))
      {
        query = query.Where(pw => pw.WareHouse.WareHouseCode == warehouseCode);
      }

      return await query.Select(pw => new ProductWareHouseDto
      {
        ProductCode = pw.Product.ProductCode ?? string.Empty,
        WareHouseCode = pw.WareHouse.WareHouseCode,
        WareHouseName = pw.WareHouse.WareHouseName,
        Quantity = pw.Quantity
      }).ToListAsync();
    }

    /// <summary>
    /// Creates a new product and stores it in the warehouse.
    /// </summary>
    /// <param name="product">The product details to be created.</param>
    /// <returns>A response indicating success or failure of the product creation process.</returns>
    public async Task<GeneralServiceResponseDto> CreateProductAsync(CreateProductDto product)
    {
      if (product == null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product cannot be null.");
      }

      var validationResponse = ValidateProduct(product);
      if (!validationResponse.IsSucceed)
      {
        return validationResponse;
      }

      var existingProduct = await this.dataContext.Products
          .Where(p => p.ProductCode == product.ProductCode)
          .FirstOrDefaultAsync();

      if (existingProduct != null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product code must be unique.");
      }

      var productNew = new Product
      {
        ProductCode = product.ProductCode,
        ProductQuantity = product.ProductQuantity,
        ProductDescription = product.ProductDescription
      };

      var warehouse = await this.GetWarehouseAsync(product.WareHouseCode);
      if (warehouse == null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 404, "Warehouse not found.");
      }
      await this.dataContext.Products.AddAsync(productNew);
      await this.dataContext.SaveChangesAsync();

      await this.CreateProductWarehouseAssociation(productNew.Id, warehouse.Value, product.ProductQuantity);

      try
      {
        await this.dataContext.SaveChangesAsync();
        return GeneralServiceResponseDto.CreateResponse(true, 200, "Product created and stored in Warehouse successfully.");
      }
      catch (Exception ex)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 500, $"An error occurred: {ex.Message}");
      }
    }

    /// <summary>
    /// Validates the product details before creation.
    /// </summary>
    /// <param name="product">The product details to validate.</param>
    /// <returns>A validation response indicating if the product is valid or not.</returns>
    private static GeneralServiceResponseDto ValidateProduct(CreateProductDto product)
    {
      if (string.IsNullOrEmpty(product.ProductCode))
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product code is required.");
      }

      if (product.ProductQuantity <= 0)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product quantity must be greater than 0.");
      }

      return GeneralServiceResponseDto.CreateResponse(true, 200, "Product is valid.");
    }

    /// <summary>
    /// Retrieves the warehouse ID for a given warehouse code.
    /// </summary>
    /// <param name="warehouseCode">The warehouse code.</param>
    /// <returns>The ID of the warehouse if found, or null if not found.</returns>
    private async Task<int?> GetWarehouseAsync(string warehouseCode)
    {
      return await this.dataContext.WareHouses
                                   .Where(w => w.WareHouseCode == warehouseCode)
                                   .Select(w => (int?)w.Id)
                                   .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Creates the association between the product and the warehouse.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="warehouseId">The warehouse ID.</param>
    /// <param name="quantity">The quantity of the product in the warehouse.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CreateProductWarehouseAssociation(int productId, int warehouseId, int quaantity)
    {
      var productWarehouse = new ProductWareHouse
      {
        ProductId = productId,
        WareHouseId = warehouseId,
        Quantity = quaantity
      };

      await this.dataContext.ProductWareHouses.AddAsync(productWarehouse);
      this.dataContext.SaveChanges();
    }
  }
}