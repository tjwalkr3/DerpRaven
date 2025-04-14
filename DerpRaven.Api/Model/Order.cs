namespace DerpRaven.Api.Model;

public class Order
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime OrderDate { get; set; }

    public User User { get; set; } = null!;
    public List<OrderedProduct> OrderedProducts { get; set; } = [];
}