using APIsEx.DTOs;
using APIsEx.Models;
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
