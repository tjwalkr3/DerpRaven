using DerpRaven.Api.Repository;
namespace DerpRaven.Api.Services;

public class ProductService : BaseClass
{
    public ProductService(AppDbContext context) : base(context)
    {
    }
}
