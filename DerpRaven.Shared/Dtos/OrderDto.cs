namespace DerpRaven.Shared.Dtos;

public class OrderDto
{

    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int UserId { get; set; }

    public List<int> ProductIds { get; set; } = [];
}