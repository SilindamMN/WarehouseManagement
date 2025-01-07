namespace Application.Interfaces
{
  using Domain.Dtos;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public interface IGenericService<TEntity, TDto>
       where TEntity : class
       where TDto : class
  {
    /// <summary> 
    /// Asynchronously creates a new entity from the provided DTO.
    //// </summary> 
    /// <param name="entityDto">The data transfer object containing the information for the entity to be created.</param>
    Task<GeneralServiceResponseDto> CreateAsync(TDto entityDto);

    /// <summary>
    ///  retrieves all entities as a collection of DTOs.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TDto>> GetAllAsync();
  }
}