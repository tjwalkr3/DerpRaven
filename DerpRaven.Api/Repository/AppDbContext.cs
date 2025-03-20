using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CustomRequest> CustomRequests { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-Many: Order <-> Product
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity<Dictionary<string, object>>(
                "OrderProduct",
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Order>()
                    .WithMany()
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade));

        // Many-to-Many: Portfolio <-> Image
        modelBuilder.Entity<Portfolio>()
            .HasMany(p => p.Images)
            .WithMany(i => i.Portfolios)
            .UsingEntity<Dictionary<string, object>>(
                "PortfolioImage",
                j => j
                    .HasOne<Image>()
                    .WithMany()
                    .HasForeignKey("ImageId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Portfolio>()
                    .WithMany()
                    .HasForeignKey("PortfolioId")
                    .OnDelete(DeleteBehavior.Cascade));

        // Many-to-Many: Product <-> Image
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Images)
            .WithMany(i => i.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductImage",
                j => j
                    .HasOne<Image>()
                    .WithMany()
                    .HasForeignKey("ImageId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade));

        // One-to-Many: CustomRequest -> User
        modelBuilder.Entity<CustomRequest>()
            .HasOne(cr => cr.User)
            .WithMany(u => u.CustomRequests)
            .HasForeignKey("UserId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: Order -> User
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey("UserId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: CustomRequest -> Type
        modelBuilder.Entity<CustomRequest>()
            .HasOne(cr => cr.Type)
            .WithMany(t => t.CustomRequests)
            .HasForeignKey("TypeId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-Many: Product -> Type
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Type)
            .WithMany(t => t.Products)
            .HasForeignKey("TypeId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-Many: Portfolio -> Type
        modelBuilder.Entity<Portfolio>()
            .HasOne(p => p.Type)
            .WithMany(t => t.Portfolios)
            .HasForeignKey("TypeId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

}
