namespace DerpRaven.Api.Model;

public class OrderedProduct
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; } = null!;

    public Order Order { get; set; } = null!;
}
