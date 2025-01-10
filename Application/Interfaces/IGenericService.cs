namespace Application.Interfaces
{
  using Domain.Dtos;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  public interface IGenericService<TEntity, TDto>
       where TEntity : class
       where TDto : class
  {
    /// <summary> 
    /// Asynchronously creates a new entity from the provided DTO.
    /// </summary> 
    /// <param name="entityDto">The data transfer object containing the information for the entity to be created.</param>
    /// <param name="existsPredicate">The predicate function to check if the entity already exists.</param>
    Task<GeneralServiceResponseDto> CreateAsync(TDto entityDto, Expression<Func<TEntity, bool>> existsPredicate);

    /// <summary>
    ///  retrieves all entities as a collection of DTOs.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TDto>> GetAllAsync();
  }
}