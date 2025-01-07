namespace Application.Services
{
  using Application.Interfaces;
  using AutoMapper;
  using Domain.Dtos;
  using Microsoft.EntityFrameworkCore;
  using Persistence;

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

    public async Task<GeneralServiceResponseDto> CreateAsync(TDto entityDto)
    {
      try
      {
        var newEntity = this.mapper.Map<TEntity>(entityDto);
        this.dataContext.Set<TEntity>().Add(newEntity);
        await this.SaveAsync();

        var createdDto = this.mapper.Map<TDto>(newEntity);
        return GeneralServiceResponseDto.CreateResponse(true, 200, "Created Successfully");
      }
      catch (Exception)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Failed to create entity");
      }
    }


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

    public async Task<IEnumerable<TDto>> GetAllAsync()
    {
      try
      {
        var entities = await this.dataContext.Set<TEntity>()
            .Where(e => EF.Property<bool>(e, "IsActive") == true)
            .ToListAsync();

        var dtos = this.mapper.Map<List<TDto>>(entities);
        return dtos;
      }
      catch (Exception ex)
      {
        return Enumerable.Empty<TDto>();
      }
    }
  }
}