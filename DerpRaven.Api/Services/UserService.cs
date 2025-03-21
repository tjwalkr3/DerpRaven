using DerpRaven.Api.Repository;
namespace DerpRaven.Api.Services;

public class UserService : BaseClass
{
    public UserService(AppDbContext context) : base(context)
    {
    }
}
