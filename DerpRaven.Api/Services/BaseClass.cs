using DerpRaven.Api.Repository;
namespace DerpRaven.Api.Services;

public class BaseClass
{
    private AppDbContext _context;

    public BaseClass(AppDbContext context)
    {
        _context = context;
    }
}
