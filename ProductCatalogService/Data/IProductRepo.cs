using DotNet.ProductCatalogService.Models;

namespace DotNet.ProductCatalogService.Data;

public interface IProductRepo
{
    bool SaveChanges();

    IEnumerable<Product?> GetAllProducts();
    Product? GetProductById(int id);
    void CreateProduct(Product? product);
}