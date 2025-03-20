namespace DerpRaven.Api.Repository;

public class PortfolioImage
{
    public int Id { get; set; }
    public Portfolio Portfolio { get; set; } = null!;
    public Image Image { get; set; } = null!;
}
