namespace DerpRaven.Api.Repository;

public class Image
{
    public int Id { get; set; }
    public string Alt { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = [];
    public List<Portfolio> Portfolios { get; set; } = [];
}
