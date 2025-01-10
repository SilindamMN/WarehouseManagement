namespace Application
{
  using AutoMapper;
  using Domain.Dtos;
  using Domain.Entities;

  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      this.CreateMap<Product, ProductDto>().ReverseMap();
      this.CreateMap<Product, CreateProductDto>().ReverseMap();
      this.CreateMap<Order, CreateOrderDto>().ReverseMap();
      this.CreateMap<Order, OrderDto>().ReverseMap();
      this.CreateMap<WareHouse, WareHouseDto>().ReverseMap();
      this.CreateMap<ProductWareHouse, ProductWareHouseDto>().ReverseMap();
    }
  }
}