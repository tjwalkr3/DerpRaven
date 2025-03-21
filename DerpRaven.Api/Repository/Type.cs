namespace DerpRaven.Api.Repository;

public class Type
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<Portfolio> Portfolios { get; set; } = [];
    public List<Product> Products { get; set; } = [];
    public List<CustomRequest> CustomRequests { get; set; } = [];
}
