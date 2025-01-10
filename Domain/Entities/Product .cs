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
    [Required(ErrorMessage = "Product code is required.")]
    [StringLength(100, ErrorMessage = "Product code cannot be longer than 100 characters.")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity available for the product. Can be nullable.
    /// </summary>
    [Required(ErrorMessage = "Product quantity is required.")]
    public int? ProductQuantity { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    [Required(ErrorMessage = "Product description is required.")]
    [StringLength(500, ErrorMessage = "Product description cannot be longer than 500 characters.")]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of product warehouse associations.
    /// </summary>
    public List<ProductWareHouse> ProductWareHouses { get; set; } = new List<ProductWareHouse>();
  }
}