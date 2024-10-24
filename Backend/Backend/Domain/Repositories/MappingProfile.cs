using AutoMapper;
using Backend.Domain.Repositories.SuperTienda;
using Backend.Models.Dtos;

namespace Backend.Domain.Repositories;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping from Entity to DTO
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDetail, OrderDetailDto>();
        CreateMap<Client, ClientDto>();
        CreateMap<Product, ProductDto>();

        // Reverse mapping from DTO to Entity
        CreateMap<OrderDto, Order>().ReverseMap();
        CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
        CreateMap<ClientDto, Client>().ReverseMap();
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}