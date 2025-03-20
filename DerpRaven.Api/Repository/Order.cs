namespace DerpRaven.Api.Repository;

public class Order
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }

    public User User { get; set; } = null!;
    public List<Product> Products { get; set; } = [];
}