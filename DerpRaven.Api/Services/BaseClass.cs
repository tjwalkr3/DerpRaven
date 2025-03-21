using DerpRaven.Api.Model;
namespace DerpRaven.Api.Services;

public class BaseClass
{
    internal AppDbContext _context;
    internal ILogger<BaseClass> _logger;

    public BaseClass(AppDbContext context, ILogger<BaseClass> logger)
    {
        _context = context;
        _logger = logger;
    }
}
