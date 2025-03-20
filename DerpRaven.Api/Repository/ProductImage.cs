namespace DerpRaven.Api.Repository;

public class ProductImage
{
    public int Id { get; set; }
    public Product Product { get; set; } = null!;
    public Image Image { get; set; } = null!;
}
