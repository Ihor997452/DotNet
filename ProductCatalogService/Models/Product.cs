using System.ComponentModel.DataAnnotations;

namespace DotNet.ProductCatalogService.Models;

public class Product
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required] 
    public string? Name { get; set; }
    [Required] 
    public string? Description { get; set; }
    [Required] 
    public decimal? Price { get; set; }
}
