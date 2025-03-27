namespace DerpRaven.Shared.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; } = null!;
    public int ProductTypeId { get; set; }

    public List<int> ImageIds { get; set; } = [];
}
