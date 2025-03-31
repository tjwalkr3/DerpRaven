namespace DerpRaven.Shared.Dtos;

public class ImageDto
{
    public int Id { get; set; }
    public string Alt { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? ImageDataUrl { get; set; }
}
