namespace WebApi.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Microsoft.AspNetCore.Mvc;
  using System.Threading.Tasks;

  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
      this.orderService = orderService;
    }

    [HttpPost("CreateOrderAndTransfer")]
    public async Task<IActionResult> CreateOrderAndTransfer([FromBody] OrderDto orderDto)
    {
      var result = await orderService.TransferProductAsync(orderDto);
      if (result.IsSucceed)
      {
        return Ok(result.Message);
      }
      return StatusCode(result.StatusCode, result.Message);
    }
  }
}
