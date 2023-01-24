using System.ComponentModel.DataAnnotations;

namespace DotNet.ProductCatalogService.DTOs;

public class CreateProductDto
{
    [Required] 
    public string? Name { get; set; }
    [Required] 
    public string? Description { get; set; }
    [Required] 
    public decimal? Price { get; set; }
}