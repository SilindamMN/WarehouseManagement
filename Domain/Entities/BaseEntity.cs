namespace Domain.Entities
{
  /// <summary>
  /// Base entity class with an Id and an IsActive flag.
  /// </summary>
  /// <typeparam name="TID">Type of the Id (primary key).</typeparam>
  public class BaseEntity<TID>
  {
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public TID? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
  }
}