namespace DerpRaven.Api.Model;

public class CustomRequest
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Status { get; set; } = null!;

    public Type Type { get; set; } = null!;
    public User User { get; set; } = null!;
}
