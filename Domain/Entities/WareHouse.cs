namespace Domain.Entities
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Represents a warehouse, including its code and name.
  /// </summary>
  public class WareHouse : BaseEntity<int>
  {
    /// <summary>
    /// Gets or sets the unique code for the warehouse.
    /// </summary>
    [Required] // Ensures that WareHouseCode is not null
    [StringLength(50, ErrorMessage = "Warehouse code cannot be longer than 50 characters.")]
    public string WareHouseCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the warehouse.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Warehouse name cannot be longer than 100 characters.")]
    public string WareHouseName { get; set; }

    /// <summary>
    /// Gets or sets the list of product warehouse associations.
    /// </summary>
    public List<ProductWareHouse> ProductWareHouses { get; set; } = new List<ProductWareHouse>();
  }
}