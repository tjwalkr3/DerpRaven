using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class ProductService : BaseClass
{
    public ProductService(AppDbContext context, ILogger<BaseClass> logger) : base(context, logger)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsByTypeAsync(string type)
    {
        return await _context.Products.Where(p => p.Type.Name == type).ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> CreateProduct(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProduct(Product product)
    {
        var oldProduct = await _context.Products.Where(p => p.Id == product.Id).FirstOrDefaultAsync();
        if (oldProduct != null)
        {
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.Type = product.Type;
            oldProduct.Images = product.Images;
            _context.Update(oldProduct);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
    }
}
