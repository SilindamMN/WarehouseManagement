namespace Application.Services
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;

  /// <summary>
  /// Service for managing product-related operations such as creating orders and retrieving order.
  /// </summary>

  public class OrderService : IOrderService
  {
    private readonly DataContext dataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context for interacting with the database.</param>
    public OrderService(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }

    /// <summary>
    /// Retrieves all orders along with product and warehouse information.
    /// </summary>
    /// <returns>A list of OrderDto objects representing all orders in the system.</returns>
    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
      var orders = await this.dataContext.Orders
          .Include(o => o.Product)
          .Include(o => o.SourceWareHouse)
              .ThenInclude(sw => sw.ProductWareHouses)
          .Include(o => o.DestinationWareHouse)
              .ThenInclude(dw => dw.ProductWareHouses)
          .Select(o => new OrderDto
          {
            SourceWareHouseName = o.SourceWareHouse.WareHouseName,
            DestinationWareHouseName = o.DestinationWareHouse.WareHouseName,
            ProductName = o.Product.ProductDescription,
            ProductQuantityOrdered = o.ProductQuantity,

            NewSourceWarehouseQuantity = o.SourceWareHouse.ProductWareHouses
                        .First(pw => pw.ProductId == o.ProductId).Quantity,

            NewDestinationWareHouseQuantity = o.DestinationWareHouse.ProductWareHouses
                        .First(pw => pw.ProductId == o.ProductId).Quantity
          })
          .ToListAsync();

      return orders;
    }

    /// <summary>
    /// Transfers a specified quantity of product from one warehouse to another.
    /// </summary>
    /// <param name="order">The order containing the transfer details.</param>
    /// <returns>A GeneralServiceResponseDto indicating the success or failure of the operation.</returns>
    public async Task<GeneralServiceResponseDto> TransferProductAsync(CreateOrderDto order)
    {
      if (order.SourceWareHouseId == order.DestinationWareHouseId)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Source and destination warehouses cannot be the same.");
      }

      using var transaction = await this.dataContext.Database.BeginTransactionAsync();
      try
      {
        var sourceProductWarehouse = await this.GetProductWarehouseAsync(order.ProductId, order.SourceWareHouseId);
        if (sourceProductWarehouse == null || sourceProductWarehouse.Quantity < order.ProductQuantity)
        {
          return GeneralServiceResponseDto.CreateResponse(false, 400, $"Insufficient stock for product {order.ProductId} in warehouse {order.SourceWareHouseId}.");
        }

        var destinationProductWarehouse = await this.GetOrCreateProductWarehouseAsync(order.ProductId, order.DestinationWareHouseId);

        UpdateWarehouseQuantities(sourceProductWarehouse, destinationProductWarehouse, order.ProductQuantity);

        await this.CreateAndSaveOrderAsync(order);

        await this.dataContext.SaveChangesAsync();

        await transaction.CommitAsync();

        var responseMessage = GenerateResponseMessage(order, sourceProductWarehouse, destinationProductWarehouse);
        return GeneralServiceResponseDto.CreateResponse(true, 200, responseMessage);
      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return GeneralServiceResponseDto.CreateResponse(false, 500, $"An error occurred: {ex.Message}");
      }
    }

    /// <summary>
    /// Retrieves the product warehouse association for a given product and warehouse.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="warehouseId">The ID of the warehouse.</param>
    /// <returns>A ProductWareHouse object representing the warehouse association, or null if not found.</returns>
    private async Task<ProductWareHouse?> GetProductWarehouseAsync(int productId, int warehouseId)
    {
      return await this.dataContext.ProductWareHouses
          .FirstOrDefaultAsync(pw => pw.ProductId == productId && pw.WareHouseId == warehouseId);
    }

    /// <summary>
    /// Retrieves or creates a product warehouse association for a given product and warehouse.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="warehouseId">The ID of the warehouse.</param>
    /// <returns>A ProductWareHouse object representing the warehouse association.</returns>
    private async Task<ProductWareHouse> GetOrCreateProductWarehouseAsync(int productId, int warehouseId)
    {
      var productWarehouse = await this.GetProductWarehouseAsync(productId, warehouseId);
      if (productWarehouse == null)
      {
        productWarehouse = new ProductWareHouse { ProductId = productId, WareHouseId = warehouseId, Quantity = 0 };
        this.dataContext.ProductWareHouses.Add(productWarehouse);
      }
      return productWarehouse;
    }

    /// <summary>
    /// Updates the quantities of products in the source and destination warehouses.
    /// </summary>
    /// <param name="source">The source product warehouse.</param>
    /// <param name="destination">The destination product warehouse.</param>
    /// <param name="quantity">The quantity of product to transfer.</param>
    private static void UpdateWarehouseQuantities(ProductWareHouse source, ProductWareHouse destination, int quantity)
    {
      source.Quantity -= quantity;
      destination.Quantity += quantity;
    }

    /// <summary>
    /// Creates a new order and saves it to the database.
    /// </summary>
    /// <param name="order">The order to be created.</param>
    private async Task CreateAndSaveOrderAsync(CreateOrderDto order)
    {
      var newOrder = new Order
      {
        ProductId = order.ProductId,
        SourceWareHouseId = order.SourceWareHouseId,
        DestinationWareHouseId = order.DestinationWareHouseId,
        ProductQuantity = order.ProductQuantity,
        OrderDate = DateTime.UtcNow
      };
      this.dataContext.Orders.Add(newOrder);
      await this.dataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Generates a response message summarizing the transfer operation.
    /// </summary>
    /// <param name="order">The order containing transfer details.</param>
    /// <param name="source">The source product warehouse.</param>
    /// <param name="destination">The destination product warehouse.</param>
    /// <returns>A string message summarizing the transfer result.</returns>
    private static string GenerateResponseMessage(CreateOrderDto order, ProductWareHouse source, ProductWareHouse destination)
    {
      return $"Product transferred successfully. " +
             $"Source Warehouse ({order.SourceWareHouseId}) now has {source.Quantity} units of product {order.ProductId}. " +
             $"Destination Warehouse ({order.DestinationWareHouseId}) now has {destination.Quantity} units of product {order.ProductId}.";
    }
  }
}