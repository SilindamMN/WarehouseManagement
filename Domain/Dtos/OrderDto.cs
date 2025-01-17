﻿namespace Domain.Dtos
{
  /// <summary>
  /// Represents the data transfer object (DTO) for an order, containing the details of the product,
  /// source and destination warehouses, and the product quantity.
  /// </summary>
  public class OrderDto
  {
    /// <summary>
    /// Gets or sets the identifier for the source warehouse of the order.
    /// </summary>
    public string? SourceWareHouseName { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the destination warehouse of the order.
    /// </summary>
    public string? DestinationWareHouseName { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the product in the order.
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the order.
    /// </summary>
    public int ProductQuantityOrdered { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the order.
    /// </summary>
    /// 
    public int NewSourceWarehouseQuantity { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the order.
    /// </summary>
    public int NewDestinationWareHouseQuantity { get; set; }
  }
}