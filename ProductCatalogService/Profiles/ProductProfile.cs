using AutoMapper;
using DotNet.ProductCatalogService.DTOs;
using DotNet.ProductCatalogService.Models;

namespace DotNet.ProductCatalogService.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Source -> Target
        CreateMap<Product, ReadProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<ReadProductDto, PublishProductDto>();
    }
}