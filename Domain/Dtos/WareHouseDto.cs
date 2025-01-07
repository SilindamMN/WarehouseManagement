namespace Domain.Dtos
{
  /// <summary>
  /// Represents the data transfer object (DTO) for a warehouse, containing details such as the warehouse code and name.
  /// </summary>
  public class WareHouseDto
  {
    /// <summary>
    /// Gets or sets the unique warehouse code.
    /// </summary>
    public string WareHouseCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the warehouse.
    /// </summary>
    public string WareHouseName { get; set; }
  }
}