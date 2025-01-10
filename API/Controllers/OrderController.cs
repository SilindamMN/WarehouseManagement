namespace WebApi.Controllers
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Microsoft.AspNetCore.Mvc;
  using System.Threading.Tasks;

  /// <summary>
  /// OrdersController handles the API endpoints related to orders, including creating and transferring orders,
  /// and retrieving all orders.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService orderService;

    /// <summary>
    /// Initializes a new instance of the OrdersController with the given IOrderService.
    /// </summary>
    /// <param name="orderService">The service responsible for handling order operations.</param>
    public OrdersController(IOrderService orderService)
    {
      this.orderService = orderService;
    }

    /// <summary>
    /// Creates a new order and transfers the product as per the provided order details.
    /// </summary>
    /// <param name="orderDto">The details of the order to be created and transferred.</param>
    /// <returns>Returns a success or failure message with an appropriate status code.</returns>
    [HttpPost("CreateOrderAndTransfer")]
    public async Task<IActionResult> CreateOrderAndTransfer([FromBody] CreateOrderDto orderDto)
    {
      var result = await this.orderService.TransferProductAsync(orderDto);
      if (result.IsSucceed)
      {
        return this.Ok(result.Message);
      }
      return this.StatusCode(result.StatusCode, result.Message);
    }

    /// <summary>
    /// Retrieves all orders from the system.
    /// </summary>
    /// <returns>Returns a list of orders, or a not found message if no orders exist.</returns>
    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
      List<OrderDto> orders = await this.orderService.GetAllOrdersAsync();
      if (orders != null && orders.Count > 0)
      {
        return this.Ok(orders);
      }
      return this.NotFound("No orders found.");
    }
  }
}