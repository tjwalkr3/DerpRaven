namespace DerpRaven.Api.Repository;

public class Portfolio
{
    public int Id { get; set; }
    public string Descriptio { get; set; } = null!;
    public string Name { get; set; } = null!;
        
    public Type Type { get; set; } = null!;
    public List<Image> Images { get; set; } = [];
}
