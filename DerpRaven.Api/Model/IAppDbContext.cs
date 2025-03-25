using Microsoft.EntityFrameworkCore;

namespace DerpRaven.Api.Model
{
    public interface IAppDbContext
    {
        DbSet<CustomRequest> CustomRequests { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Portfolio> Portfolios { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}