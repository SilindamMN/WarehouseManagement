namespace Domain.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Represents an order, linking a product and its associated warehouses (source and destination).
  /// </summary>
  public class Order : BaseEntity<int>
  {
    /// <summary>
    /// Gets or sets the product identifier associated with the order.
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product associated with the order.
    /// </summary>
    [Required(ErrorMessage = "Product is required.")]
    public Product? Product { get; set; }

    /// <summary>
    /// Gets or sets the source warehouse identifier where the product is sourced from.
    /// </summary>
    [Required]
    public int SourceWareHouseId { get; set; }

    /// <summary>
    /// Gets or sets the source warehouse for the order.
    /// </summary>
    [Required(ErrorMessage = "Source Warehouse is required.")]
    public WareHouse? SourceWareHouse { get; set; }

    /// <summary>
    /// Gets or sets the destination warehouse identifier where the product is shipped to.
    /// </summary>
    [Required]
    public int DestinationWareHouseId { get; set; }

    /// <summary>
    /// Gets or sets the destination warehouse for the order.
    /// </summary>
    [Required(ErrorMessage = "Destination Warehouse is required.")]
    public WareHouse? DestinationWareHouse { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product ordered.
    /// </summary>
    [Required]
    public int ProductQuantity { get; set; }

    /// <summary>
    /// Gets or sets the date when the order was placed.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }
  }
}