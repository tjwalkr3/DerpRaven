namespace DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _blobService;

    public ImageController(IImageService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromBody] UploadImageRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = request.File.OpenReadStream();
        await _blobService.UploadImageAsync(request.File.FileName, request.Description, stream);

        return Ok("Uploaded");
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListImages()
    {
        var images = await _blobService.ListImagesAsync();
        return Ok(images);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetImage(int id)
    {
        string imageName = await _blobService.GetFileName(id);
        var image = await _blobService.GetImageAsync(id);
        if (image == null) return NotFound("An image with this ID was not found!");
        return File(image, "image/png", imageName);
    }

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        bool result = await _blobService.DeleteImageAsync(id);
        if (!result) return BadRequest("Failed to delete image.");
        return Ok("Deleted");
    }
}

public class UploadImageRequest
{
    public IFormFile File { get; set; } = null!;
    public string Description { get; set; } = null!;
}