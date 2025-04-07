using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace DerpRaven.Api.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string OAuth { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool Active { get; set; } = false;

    public List<CustomRequest> CustomRequests { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}
