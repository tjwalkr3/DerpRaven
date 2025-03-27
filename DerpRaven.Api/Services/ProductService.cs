using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class ProductService : IProductService
{
    private AppDbContext _context;
    private ILogger _logger;

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Select(p => MapToProductDto(p))
            .ToListAsync();
    }

    public async Task<List<ProductDto>> GetProductsByTypeAsync(string productType)
    {
        return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Where(p => p.ProductType.Name == productType)
            .Select(p => MapToProductDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Where(p => p.Id == id)
            .Select(p => MapToProductDto(p))
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProductDto>> GetProductsByNameAsync(string name)
    {
        string searchQuery = name.Trim().ToLower();
        return await _context.Products
            .Where(p => p.Name.Trim().ToLower().Contains(searchQuery))
            .Select(p => MapToProductDto(p))
            .ToListAsync();
    }

    public async Task<bool> CreateProductAsync(ProductDto dto)
    {
        var product = await MapFromProductDto(dto);
        if (product != null)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateProductAsync(ProductDto dto)
    {
        var oldProduct = await _context.Products.FindAsync(dto.Id);
        var productType = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        var images = await _context.Images
            .Where(i => dto.ImageIds.Contains(i.Id))
            .ToListAsync();

        if (oldProduct != null && productType != null)
        {
            oldProduct.Name = dto.Name;
            oldProduct.Price = dto.Price;
            oldProduct.Quantity = dto.Quantity;
            oldProduct.Description = dto.Description;
            oldProduct.ProductType = productType;
            oldProduct.Images = images;
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<Product?> MapFromProductDto(ProductDto dto)
    {
        var productType = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        var images = await _context.Images
            .Where(i => dto.ImageIds.Contains(i.Id))
            .ToListAsync();
        if (productType == null) return null;

        return new Product()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            Description = dto.Description,
            ProductType = productType,
            Images = images,
            Orders = []
        };
    }

    private static ProductDto MapToProductDto(Product product)
    {
        List<int> imageIds = product.Images
            .Select(p => p.Id)
            .ToList();

        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            ProductTypeId = product.ProductType.Id,
            ImageIds = imageIds
        };
    }
}
