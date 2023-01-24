using DotNet.ProductCatalogService.Models;

namespace DotNet.ProductCatalogService.Data;

public class ProductRepo : IProductRepo
{
    private readonly AppDbContext _context;

    public ProductRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }

    public IEnumerable<Product?> GetAllProducts()
    {
        return _context.Products!.ToList();
    }

    public Product? GetProductById(int id)
    {
        return _context.Products!.FirstOrDefault(p => p.Id == id);
    }

    public void CreateProduct(Product? product)
    {
        if (product != null) _context.Products!.Add(product);
    }
}