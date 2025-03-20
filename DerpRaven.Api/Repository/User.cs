namespace DerpRaven.Api.Repository;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OAuth { get; set; } = string.Empty;
    public string Email {  get; set; } = string.Empty;
    public bool Active = false;

    public List<CustomRequest> CustomRequests { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}
