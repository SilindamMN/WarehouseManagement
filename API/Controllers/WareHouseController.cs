namespace API.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.AspNetCore.Mvc;
  using System.Linq.Expressions;

  /// <summary>
  /// WareHouseController handles the API endpoints related to warehouse products, including retrieving all products
  /// in a warehouse and creating a new warehouse, ensuring that the warehouse code is unique.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class WareHouseController : ControllerBase
  {
    private readonly IGenericService<WareHouse, WareHouseDto> genericService;

    /// <summary>
    /// Initializes a new instance of the WareHouseController with the given generic service.
    /// </summary>
    /// <param name="genericService">The generic service for warehouse-related operations.</param>
    public WareHouseController(IGenericService<WareHouse, WareHouseDto> genericService)

    {
      this.genericService = genericService;
    }

    /// <summary>
    /// Retrieves all warehouse products.
    /// </summary>
    /// <returns>An HTTP response with the list of all products in the warehouse.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var result = await this.genericService.GetAllAsync();

      return this.Ok(result);
    }

    /// <summary>
    /// Creates a new warehouse if it doesn't already exist based on the warehouse code.
    /// </summary>
    /// <param name="warehouse">The DTO representing the warehouse to be created.</param>
    /// <returns>An HTTP response indicating the success or failure of the warehouse creation.</returns>
    [HttpPost("CreateWareHouse")]
    public async Task<IActionResult> CreateWareHouse([FromBody] WareHouseDto warehouse)
    {
      Expression<Func<WareHouse, bool>> existsPredicate = e => e.WareHouseCode == warehouse.WareHouseCode;

      var result = await this.genericService.CreateAsync(warehouse, existsPredicate);

      if (result.IsSucceed)
      {
        return this.Ok(result.Message);
      }
      return this.StatusCode(result.StatusCode, result.Message);
    }
  }
}