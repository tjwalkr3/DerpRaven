namespace DerpRaven.Api.Services;
using Azure;
using DerpRaven.Api.Model;
using DerpRaven.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

public class ImageService : IImageService
{
    private readonly IBlobService _blobService;
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public ImageService(IBlobService blobService, AppDbContext context, ILogger<ImageService> logger)
    {
        _blobService = blobService;
        _context = context;
        _logger = logger;
    }

    public static ImageDto MapToImageDto(ImageEntity image)
    {
        return new()
        {
            Id = image.Id,
            Path = image.Path,
            Alt = image.Alt
        };
    }

    public async Task<bool> UploadImageAsync(string fileName, string alt, Stream stream)
    {
        _logger.LogInformation("Uploading image {FileName} with alt text \"{AltText}\"", fileName, alt);
        ImageEntity image = new() { Path = fileName, Alt = alt, Portfolios = [], Products = [] };
        if (stream != null)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            await _blobService.CreateIfNotExistsAsync();
            await _blobService.UploadAsync(image.Id.ToString(), stream);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<string> GetFileName(int id)
    {
        _logger.LogInformation("Fetching file name for image with ID {ImageId}", id);
        var image = await _context.Images.FindAsync(id);
        if (image != null)
        {
            return image.Path;
        }
        return "";
    }

    public async Task<List<ImageDto>> ListImagesAsync()
    {
        _logger.LogInformation("Fetching all images from the database");
        return await _context.Images
            .Select(i => MapToImageDto(i))
            .ToListAsync();
    }

    public async Task<Stream?> GetImageAsync(int id)
    {
        _logger.LogInformation("Fetching image with ID {ImageId} from blob storage", id);
        if (!await _context.Images.AnyAsync(i => i.Id == id))
        {
            _logger.LogWarning($"Image {id} not found in the database.");
            return null;
        }

        try
        {
            return await _blobService.DownloadAsync(id.ToString());
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, $"Error getting image {id} from blob storage.");
            return null;
        }
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        _logger.LogInformation("Deleting image with ID {ImageId}", id);
        var image = await _context.Images.FindAsync(id);
        if (image != null)
        {
            // delete the image in the database
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            // delete the image in the blob storage
            await _blobService.DeleteAsync(id.ToString());

            return true;
        }

        return false;
    }

    public async Task<bool> UpdateImageDescriptionAsync(int id, string alt)
    {
        _logger.LogInformation("Updating image description for image with ID {ImageId}", id);
        var image = await _context.Images.FindAsync(id);
        if (image != null)
        {
            // update the image alt text in the database
            image.Alt = alt;
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<ImageDto?> GetImageInfoAsync(int id)
    {
        _logger.LogInformation("Fetching image info for image with ID {ImageId}", id);
        var image = await _context.Images.FindAsync(id);
        if (image != null) return MapToImageDto(image);
        return null;
    }

    public async Task<List<ImageDto>> GetInfoForImagesAsync(List<int> ids)
    {
        _logger.LogInformation("Fetching image info for images with IDs {ImageIds}", string.Join(", ", ids));
        List<ImageDto> images = [];
        foreach (var id in ids)
        {
            ImageDto? dto = await GetImageInfoAsync(id);
            if (dto != null) images.Add(dto);
        }
        return images;
    }
}
