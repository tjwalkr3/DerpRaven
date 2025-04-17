﻿namespace DerpRaven.Api.Controllers;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IDerpRavenMetrics _metrics;

    public ImageController(IImageService blobService, IDerpRavenMetrics metrics)
    {
        _imageService = blobService;
        _metrics = metrics;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        bool result = await _imageService.UploadImageAsync(file.FileName, description, stream);

        if (result) return Ok("Uploaded");

        _metrics.AddImageEndpointCall();
        return StatusCode(500, "An error occurred while uploading the image.");
    }

    [HttpGet("get/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImage(int id)
    {
        string imageName = await _imageService.GetFileName(id);
        var image = await _imageService.GetImageAsync(id);
        if (image == null) return NotFound("An image with this ID was not found!");
        _metrics.AddImageEndpointCall();
        return File(image, "image/png", imageName);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        bool result = await _imageService.DeleteImageAsync(id);
        if (!result) return BadRequest("Failed to delete image.");
        _metrics.AddImageEndpointCall();
        return Ok("Deleted");
    }


    [HttpGet("list")]
    [AllowAnonymous]
    public async Task<IActionResult> ListImages()
    {
        var images = await _imageService.ListImagesAsync();
        _metrics.AddImageEndpointCall();
        return Ok(images);
    }

    [HttpGet("info/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImageInfo(int id)
    {
        var image = await _imageService.GetImageInfoAsync(id);
        if (image == null) return NotFound("An image with this ID was not found!");
        _metrics.AddImageEndpointCall();
        return Ok(image);
    }

    [HttpGet("info-many")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImageInfoMany([FromQuery] List<int> ids)
    {
        var imageDtos = await _imageService.GetInfoForImagesAsync(ids);
        if (imageDtos == null) return NotFound("Images with these IDs were not found!");
        _metrics.AddImageEndpointCall();
        return Ok(imageDtos);
    }
}
