namespace DerpRaven.Shared.Dtos;

public class PortfolioDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int ProductTypeId { get; set; }

    public List<int> ImageIds { get; set; } = [];
}
