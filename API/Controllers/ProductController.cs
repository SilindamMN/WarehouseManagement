namespace API.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// ProductController handles the API endpoints related to products, including searching products by code,
  /// retrieving all products, and creating new products.
  /// </summary>
  [Route("api/[controller]")]
  public class ProductController : ControllerBase
  {
    private readonly IGenericService<Product, ProductDto> genericService;
    private readonly IProductService productService;

    /// <summary>
    /// Initializes a new instance of the ProductController with the given services.
    /// </summary>
    /// <param name="genericService">The generic service for product-related operations.</param>
    /// <param name="productService">The product service for specific product operations.</param>
    public ProductController(IGenericService<Product, ProductDto> genericService, IProductService productService)

    {
      this.genericService = genericService;
      this.productService = productService;
    }

    /// <summary>
    /// Retrieves products based on either product code or warehouse code.
    /// </summary>
    /// <param name="productCode">The product code to search for (optional).</param>
    /// <param name="warehouseCode">The warehouse code to search for (optional).</param>
    /// <returns>An HTTP response with the list of products matching the provided codes.</returns>
    [HttpGet("Search")]
    public async Task<IActionResult> GetProductsByProductCodeOrWarehouseCode([FromQuery] string? productCode, [FromQuery] string? warehouseCode)
    {
      var products = await this.productService.GetProductsByProductCodeOrWarehouseCodeAsync(productCode, warehouseCode);
      if (products == null || products.Count == 0)
      {
        return this.Ok(products);
      }
      return this.Ok(products);
    }

    /// <summary>
    /// Retrieves all products from the system.
    /// </summary>
    /// <returns>An HTTP response with the list of all products.</returns>
    [HttpGet("all-products")]
    public async Task<IActionResult> GetAllProducts()
    {
      var result = await this.genericService.GetAllAsync();

      return this.Ok(result);
    }

    /// <summary>
    /// Creates a new product in the system.
    /// </summary>
    /// <param name="product">The DTO representing the product to be created.</param>
    /// <returns>An HTTP response indicating the success or failure of the product creation.</returns>
    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
      var result = await this.productService.CreateProductAsync(product);
      if (result.IsSucceed)
      {
        return this.Ok(result.Message);
      }
      return this.StatusCode(result.StatusCode, result.Message);
    }
  }
}