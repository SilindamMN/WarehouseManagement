namespace Domain.Dtos
{
  /// <summary>
  /// Represents the data transfer object (DTO) for the relationship between a product and a warehouse,
  /// including product and warehouse details, as well as the quantity of the product in the warehouse.
  /// </summary>
  public class ProductWareHouseDto
  {
    /// <summary>
    /// Gets or sets the unique product code.
    /// </summary>
    public string ProductCode { get; set; }

    /// <summary>
    /// Gets or sets the unique warehouse code.
    /// </summary>
    public string WareHouseCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the warehouse.
    /// </summary>
    public string WareHouseName { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product in the warehouse.
    /// </summary>
    public int Quantity { get; set; }
  }
}