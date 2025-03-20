namespace DerpRaven.Api.Repository;

public class OrderProduct
{
    public int Id { get; set; }
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
