namespace DerpRaven.Api.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; } = null!;

    public ProductType ProductType { get; set; } = null!;
    public List<ImageEntity> Images { get; set; } = [];
}
