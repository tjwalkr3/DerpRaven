using DerpRaven.Api.Model;

namespace DerpRaven.Api.Dtos;

public class ImageDto
{
    public int Id { get; set; }
    public string Alt { get; set; } = null!;
    public string Path { get; set; } = null!;
}
