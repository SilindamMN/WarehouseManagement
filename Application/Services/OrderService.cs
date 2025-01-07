namespace Application.Services
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;

  public class OrderService : IOrderService
  {
    private readonly DataContext dataContext;

    public OrderService(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }
    public async Task<GeneralServiceResponseDto> TransferProductAsync(OrderDto order)
    {
      if (order.SourceWareHouseId == order.DestinationWareHouseId)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Source and destination warehouses cannot be the same.");
      }

      using var transaction = await dataContext.Database.BeginTransactionAsync();
      try
      {
        var sourceProductWarehouse = await GetProductWarehouseAsync(order.ProductId, order.SourceWareHouseId);
        if (sourceProductWarehouse == null || sourceProductWarehouse.Quantity < order.ProductQuantity)
        {
          return GeneralServiceResponseDto.CreateResponse(false, 400, $"Insufficient stock for product {order.ProductId} in warehouse {order.SourceWareHouseId}.");
        }

        var destinationProductWarehouse = await GetOrCreateProductWarehouseAsync(order.ProductId, order.DestinationWareHouseId);

        // Update warehouse quantities first
        UpdateWarehouseQuantities(sourceProductWarehouse, destinationProductWarehouse, order.ProductQuantity);

        // Create and add the order// Create and add the order
        await CreateAndSaveOrderAsync(order);


        // Save changes after all updates
        await dataContext.SaveChangesAsync(); // Save all changes at once

        // Commit transaction
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


    private async Task<ProductWareHouse> GetProductWarehouseAsync(int productId, int warehouseId)
    {
      return await dataContext.ProductWareHouses
          .FirstOrDefaultAsync(pw => pw.ProductId == productId && pw.WareHouseId == warehouseId);
    }

    private async Task<ProductWareHouse> GetOrCreateProductWarehouseAsync(int productId, int warehouseId)
    {
      var productWarehouse = await GetProductWarehouseAsync(productId, warehouseId);
      if (productWarehouse == null)
      {
        productWarehouse = new ProductWareHouse { ProductId = productId, WareHouseId = warehouseId, Quantity = 0 };
        dataContext.ProductWareHouses.Add(productWarehouse);
      }
      return productWarehouse;
    }

    private void UpdateWarehouseQuantities(ProductWareHouse source, ProductWareHouse destination, int quantity)
    {
      source.Quantity -= quantity;
      destination.Quantity += quantity;
    }

    private async Task CreateAndSaveOrderAsync(OrderDto order)
    {
      var newOrder = new Order
      {
        ProductId = order.ProductId,
        SourceWareHouseId = order.SourceWareHouseId,  // Use the existing property from OrderDto
        DestinationWareHouseId = order.DestinationWareHouseId,  // Use the existing property from OrderDto
        ProductQuantity = order.ProductQuantity,
        OrderDate = DateTime.UtcNow
      };
      dataContext.Orders.Add(newOrder);
      await dataContext.SaveChangesAsync();
    }


    private string GenerateResponseMessage(OrderDto order, ProductWareHouse source, ProductWareHouse destination)
    {
      return $"Product transferred successfully. " +
             $"Source Warehouse ({order.SourceWareHouseId}) now has {source.Quantity} units of product {order.ProductId}. " +
             $"Destination Warehouse ({order.DestinationWareHouseId}) now has {destination.Quantity} units of product {order.ProductId}.";
    }
  }
}