namespace Application.Interfaces
{
  using Domain.Dtos;
  using System.Threading.Tasks;

  /// <summary>
  /// IOrder Interface
  /// </summary>
  public interface IOrderService
  {
    /// <summary>
    ///  transfers a product based on the details provided in the order DTO.
    /// </summary>
    /// <param name="order">The data transfer object containing the details of the order for the product transfer.</param>
    /// <returns>A task representing the asynchronous operation, containing a general service response DTO indicating the result of product transfer.</returns>
    Task<GeneralServiceResponseDto> TransferProductAsync(OrderDto order);
  }
}