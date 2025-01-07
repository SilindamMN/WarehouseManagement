namespace Domain.Entities
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Represents a product, including its code, description, and available quantity.
  /// </summary>
  public class Product : BaseEntity<int>
  {
    /// <summary>
    /// Gets or sets the unique product code.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity available for the product. Can be nullable.
    /// </summary>
    [Required]
    public int? ProductQuantity { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [Required]
    [StringLength(500)]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of product warehouse associations.
    /// </summary>
    public List<ProductWareHouse> ProductWareHouses { get; set; } = new List<ProductWareHouse>();
  }
}