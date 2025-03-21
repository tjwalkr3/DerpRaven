using DerpRaven.Api.Repository;
namespace DerpRaven.Api.Services;

public class OrderService : BaseClass
{
    public OrderService(AppDbContext context) : base(context)
    {
    }
}

