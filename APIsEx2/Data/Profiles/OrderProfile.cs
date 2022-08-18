using APIsEx2.Models;
using APIsEx2.DTOs;
using AutoMapper;

namespace APIsEx2.Data.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(o=>o.Products,c=>c.MapFrom(e=>e.OrderProducts))
                .ForMember(o=>o.Name,c=>c.MapFrom(e=>e.Customer.Name))
                .ForMember(o=>o.Phone,c=>c.MapFrom(e=>e.Customer.Phone))
                .ReverseMap();
            CreateMap<OrderProduct, OrderProductDto>()
                .ForMember(o => o.UnitPrice, c => c.MapFrom(e => e.Product.UnitPrice))
                .ForMember(o => o.Name, c => c.MapFrom(e => e.Product.Name))
                .ForMember(o => o.Description, c => c.MapFrom(e => e.Product.Description)).ReverseMap();
        }
    }

}
