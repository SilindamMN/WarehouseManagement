namespace API.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.AspNetCore.Mvc;

  [Route("api/[controller]")]
  [ApiController]
  public class WareHouseController : ControllerBase
  {
    private readonly IGenericService<WareHouse, WareHouseDto> genericService;

    public WareHouseController(IGenericService<WareHouse, WareHouseDto> genericService)

    {
      this.genericService = genericService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var result = await genericService.GetAllAsync();

      return Ok(result);
    }

    [HttpPost("CreateWareHouse")]
    public async Task<IActionResult> CreateWareHouse([FromBody] WareHouseDto warehouse)
    {
      var result = await this.genericService.CreateAsync(warehouse);
      if (result.IsSucceed)
      {
        return Ok(result.Message);
      }
      return this.StatusCode(result.StatusCode, result.Message);
    }
  }
}