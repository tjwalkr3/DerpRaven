namespace DerpRaven.Api.Model;

public class ImageEntity
{
    public int Id { get; set; }
    public string Alt { get; set; } = null!;
    public string Path { get; set; } = null!;

    public List<Product> Products { get; set; } = [];
    public List<Portfolio> Portfolios { get; set; } = [];
}
