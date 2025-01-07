namespace Domain.Entities
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Represents the relationship between a product and a warehouse, including the stock quantity.
  /// </summary>
  public class ProductWareHouse : BaseEntity<int>
  {
    /// <summary>
    /// Gets or sets the product identifier.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product associated with this warehouse.
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    [Required]
    public int WareHouseId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse associated with this product.
    /// </summary>
    public WareHouse WareHouse { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the warehouse.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
    public int Quantity { get; set; }
  }
}