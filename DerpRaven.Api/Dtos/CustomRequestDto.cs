namespace DerpRaven.Api.Dtos;

public class CustomRequestDto
{
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ProductTypeId { get; set; }
    public int UserId { get; set; }
}
