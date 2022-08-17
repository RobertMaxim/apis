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
                .ForMember(o=>o.Name,c=>c.MapFrom(e=>e.Customer.Name))
                .ForMember(o=>o.Phone,c=>c.MapFrom(e=>e.Customer.Phone))
                .ReverseMap();
        }
    }

}
