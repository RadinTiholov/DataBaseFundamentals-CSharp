using AutoMapper;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<InputProductDTO, Product>();
            CreateMap<InputCategoriesDTO, Category>();
            CreateMap<InputCatAndProdDTO, CategoryProduct>();
        }
    }
}
