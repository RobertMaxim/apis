using APIsEx.DTOs;
using APIsEx2.Models;
using AutoMapper;

namespace APIsEx.Data.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
