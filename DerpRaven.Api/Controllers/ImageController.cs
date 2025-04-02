namespace DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(IImageService blobService)
    {
        _imageService = blobService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        bool result = await _imageService.UploadImageAsync(file.FileName, description, stream);

        if (result) return Ok("Uploaded");

        return StatusCode(500, "An error occurred while uploading the image.");
    }

    [HttpGet("list")]
    [AllowAnonymous]
    public async Task<IActionResult> ListImages()
    {
        var images = await _imageService.ListImagesAsync();
        return Ok(images);
    }

    [HttpGet("get/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(int id)
    {
        string imageName = await _imageService.GetFileName(id);
        var image = await _imageService.GetImageAsync(id);
        if (image == null) return NotFound("An image with this ID was not found!");
        return File(image, "image/png", imageName);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        bool result = await _imageService.DeleteImageAsync(id);
        if (!result) return BadRequest("Failed to delete image.");
        return Ok("Deleted");
    }

    [HttpGet("info/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImageInfo(int id)
    {
        var image = await _imageService.GetImageInfoAsync(id);
        if (image == null) return NotFound("An image with this ID was not found!");
        return Ok(image);
    }
}
