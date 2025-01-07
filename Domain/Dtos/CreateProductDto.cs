namespace Domain.Dtos
{
  /// <summary>
  /// Represents the data transfer object (DTO) for a product, containing details such as the product code,
  /// product quantity, and product description.
  /// </summary>
  public class CreateProductDto
  {
    /// <summary>
    /// Gets or sets the unique product code.
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int ProductQuantity { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public string WareHouseCode { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;
  }
}