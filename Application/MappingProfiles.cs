namespace Application
{
  using AutoMapper;
  using Domain.Dtos;
  using Domain.Entities;

  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductDto>().ReverseMap();
      CreateMap<Product, CreateProductDto>().ReverseMap();
      CreateMap<Order, OrderDto>().ReverseMap();
      this.CreateMap<WareHouse, WareHouseDto>().ReverseMap();
      this.CreateMap<ProductWareHouse, ProductWareHouseDto>().ReverseMap();
    }
  }
}