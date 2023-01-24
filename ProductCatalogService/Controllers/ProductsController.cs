using AutoMapper;
using DotNet.ProductCatalogService.Data;
using DotNet.ProductCatalogService.DTOs;
using DotNet.ProductCatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.ProductCatalogService.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductRepo _repo;
    private readonly IMapper _mapper;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductRepo repo,
        IMapper mapper)
    {
        _logger = logger;
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        _logger.LogInformation("--> Getting all Products");
        return Ok(_mapper.Map<IEnumerable<ReadProductDto>>(_repo.GetAllProducts()));
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public ActionResult<Product> GetProduct(int id)
    {
        _logger.LogInformation($"--> Getting Product by ID: {id}");
        var product = _repo.GetProductById(id);
        return product == null ? NotFound() : Ok(_mapper.Map<ReadProductDto>(product));
    }

    [HttpPost]
    public ActionResult<ReadProductDto> CreateProduct(CreateProductDto createProductDto)
    {
        _logger.LogInformation($"--> Creating Product: {createProductDto}");
        var product = _mapper.Map<Product>(createProductDto);
        _repo.CreateProduct(product);
        _repo.SaveChanges();

        var productReadDto = _mapper.Map<ReadProductDto>(product);

        return CreatedAtRoute(nameof(GetProduct), new { productReadDto.Id }, productReadDto);
    }

}