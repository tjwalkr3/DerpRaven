namespace DerpRaven.Api.Repository;

public class CustomRequest
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Type Type { get; set; } = null!;
    public User User { get; set; } = null!;
}
