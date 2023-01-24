namespace DotNet.ProductCatalogService.DTOs;

public class PublishProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Event { get; set; }
}