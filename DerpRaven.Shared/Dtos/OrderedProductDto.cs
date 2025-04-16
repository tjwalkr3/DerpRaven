namespace DerpRaven.Shared.Dtos;

public class OrderedProductDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; } = null!;

    public decimal ItemTotal => Price * Quantity;

    public int OrderID { get; set; }
}
