namespace DerpRaven.Api.Model;

public class CustomRequest
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Status { get; set; } = null!;

    public ProductType ProductType { get; set; } = null!;
    public User User { get; set; } = null!;
}
