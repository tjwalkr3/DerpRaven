namespace DerpRaven.Api.Repository;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; } = null!;

    public Type Type { get; set; } = null!;
    public List<Image> Images { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}
