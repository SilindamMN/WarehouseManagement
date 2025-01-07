namespace API.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.AspNetCore.Mvc;

  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IGenericService<Product, ProductDto> genericService;
    private readonly IProductService productService;

    public ProductController(IGenericService<Product, ProductDto> genericService, IProductService productService)

    {
      this.genericService = genericService;
      this.productService = productService;
    }

    [HttpGet("Search")]
    public async Task<IActionResult> GetProductsByProductCodeOrWarehouseCode([FromQuery] string? productCode, [FromQuery] string? warehouseCode)
    {
      var products = await productService.GetProductsByProductCodeOrWarehouseCodeAsync(productCode, warehouseCode);
      if (products == null || products.Count == 0)
      {
        return Ok(products);
      }
      return Ok(products);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var result = await genericService.GetAllAsync();

      return Ok(result);
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
    {
      var result = await productService.CreateProductAsync(product);
      if (result.IsSucceed)
      {
        return Ok(result.Message);
      }
      return StatusCode(result.StatusCode, result.Message);
    }
  }
}