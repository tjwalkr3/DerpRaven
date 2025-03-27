namespace DerpRaven.Shared.Dtos;

public class CustomRequestDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int ProductTypeId { get; set; }
    public int UserId { get; set; }
}
