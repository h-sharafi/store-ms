


using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;


public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var res = await _catalogContext.Products.DeleteOneAsync(c => c.Id == id);
        return res.IsAcknowledged && res.DeletedCount > 0;
    }

    public async Task<Product> GetProduct(string id)
    {
        var model = await _catalogContext.Products.Find(c => c.Id == id)
        .FirstOrDefaultAsync();
        return model;
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        var filter = Builders<Product>.Filter.ElemMatch(c => c.Category, categoryName);
        var models = await _catalogContext.Products.Find(filter).ToListAsync();
        return models;
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.ElemMatch(c => c.Name, name);
        var models = await _catalogContext.Products.Find(filter).ToListAsync();
        return models;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _catalogContext.Products.Find(c => true)
        .ToListAsync();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var res = await _catalogContext.Products.ReplaceOneAsync(filter: c => c.Id == product.Id, replacement: product);
        return res.IsAcknowledged && res.MatchedCount > 0;
    }
}
