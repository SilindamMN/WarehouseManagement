namespace Application.Services
{
  using Application.Interfaces;
  using AutoMapper;
  using Domain.Dtos;
  using Microsoft.EntityFrameworkCore;
  using Persistence;
  using System.Linq.Expressions;

  public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto>
        where TEntity : class
        where TDto : class
  {
    private readonly DataContext dataContext;
    private readonly IMapper mapper;

    public GenericService(DataContext dataContext, IMapper mapper)
    {
      this.dataContext = dataContext;
      this.mapper = mapper;
    }

    /// <summary>
    /// Creates a new entity if it does not already exist based on the provided predicate.
    /// </summary>
    /// <param name="entityDto">The DTO representing the entity to be created.</param>
    /// <param name="existsPredicate">The predicate used to check if the entity already exists.</param>
    /// <returns>A GeneralServiceResponseDto indicating the success or failure of the creation.</returns>
    public async Task<GeneralServiceResponseDto> CreateAsync(TDto entityDto, Expression<Func<TEntity, bool>> existsPredicate)
    {
      try
      {
        var existingEntity = await this.dataContext.Set<TEntity>().FirstOrDefaultAsync(existsPredicate);

        if (existingEntity != null)
        {
          return GeneralServiceResponseDto.CreateResponse(false, 400, "Entity already exists");
        }

        var newEntity = this.mapper.Map<TEntity>(entityDto);
        this.dataContext.Set<TEntity>().Add(newEntity);
        await this.SaveAsync();

        var createdDto = this.mapper.Map<TDto>(newEntity);
        return GeneralServiceResponseDto.CreateResponse(true, 200, "Created Successfully");
      }
      catch (Exception ex)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, $"Failed to create entity: {ex.Message}");
      }
    }

    /// <summary>
    /// Saves changes made to the data context.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SaveAsync()
    {
      try
      {
        await this.dataContext.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        GeneralServiceResponseDto.CreateResponse(false, 400, ex.Message);
      }
    }

    /// <summary>
    /// Retrieves all active entities and maps them to DTOs.
    /// </summary>
    /// <returns>A collection of DTOs representing all active entities in the system.</returns>
    public async Task<IEnumerable<TDto>> GetAllAsync()
    {
      try
      {
        var entities = await this.dataContext.Set<TEntity>()
            .Where(e => EF.Property<bool>(e, "IsActive"))
            .ToListAsync();

        var dtos = this.mapper.Map<List<TDto>>(entities);
        return dtos;
      }
      catch (Exception)
      {
        return Enumerable.Empty<TDto>();
      }
    }
  }
}