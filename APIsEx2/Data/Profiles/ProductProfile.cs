using APIsEx.DTOs;
using APIsEx2.Models;
using AutoMapper;

namespace APIsEx.Data.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap().ForAllMembers(p => p.Condition((src, dest, srcMember) => srcMember!=default));
            //CreateMap<Product, ProductDto>().ForAllMembers(p => p.Condition((src, dest, srcMember) => srcMember != default));
        }
    }
}
