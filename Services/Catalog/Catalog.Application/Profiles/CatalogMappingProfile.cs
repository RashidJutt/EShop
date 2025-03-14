using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Core.Common;
using Catalog.Core.Entities;

namespace Catalog.Application.Profiles;

public class CatalogMappingProfile : Profile
{
    public CatalogMappingProfile()
    {
        CreateMap<ProductBrand, BrandDto>().ReverseMap();
        CreateMap<ProductType, TypeDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>()
            .ForMember(x=>x.Id,cfg=>cfg.Ignore());
        CreateMap<PagedResult<Product>, PagedResult<ProductDto>>();
    }
}
