namespace Application.Interfaces
{
  using Domain.Dtos;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  /// <summary>
  /// Interface for handling product-related operations in the application.
  /// </summary>
  public interface IProductService
  {
    /// <summary>
    /// Creates a new product asynchronously.
    /// </summary>
    /// <param name="product">The product to be created.</param>
    /// <returns>A response containing the result of the creation operation.</returns>
    Task<GeneralServiceResponseDto> CreateProductAsync(CreateProductDto product);

    /// <summary>
    /// Retrieves a list of products based on the product code or warehouse code.
    /// </summary>
    /// <param name="productCode">The product code to filter by.</param>
    /// <param name="warehouseCode">The warehouse code to filter by.</param>
    /// <returns>A list of products matching the provided criteria.</returns>
    Task<List<ProductWareHouseDto>> GetProductsByProductCodeOrWarehouseCodeAsync(string? productCode, string? warehouseCode);
  }
}