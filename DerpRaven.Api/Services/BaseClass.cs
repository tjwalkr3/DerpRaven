using DerpRaven.Api.Model;
namespace DerpRaven.Api.Services;

public class BaseClass
{
    internal AppDbContext _context;
    internal ILogger _logger;

    public BaseClass(AppDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }
}
