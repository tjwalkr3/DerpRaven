using DerpRaven.Api.Model;

namespace DerpRaven.Api.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string OAuth { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool Active { get; set; } = false;

    public List<int> OrderIds { get; set; } = [];
}
