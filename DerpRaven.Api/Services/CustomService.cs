using DerpRaven.Api.Repository;
namespace DerpRaven.Api.Services;

public class CustomService : BaseClass
{
    public CustomService(AppDbContext context) : base(context)
    {
    }
}
