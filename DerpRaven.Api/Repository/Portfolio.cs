namespace DerpRaven.Api.Repository;

public class Portfolio
{
    public int Id { get; set; }
    public string Descriptio { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Type Type { get; set; } = null!;
}
