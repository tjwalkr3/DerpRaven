namespace DerpRaven.Api.Services;
using Azure;
using Azure.Storage.Blobs;
using DerpRaven.Api.Model;
using DerpRaven.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class ImageService : IImageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly AppDbContext _context;
    private readonly ILogger _logger;

    public ImageService(IOptions<BlobStorageOptions> options, AppDbContext context, ILogger<ImageService> logger)
    {
        string connectionString = options.Value.ConnectionString
            ?? throw new ArgumentNullException("BlobStorage connection string is missing!");
        string containerName = options.Value.ContainerName;
        BlobServiceClient _blobClient = new BlobServiceClient(connectionString);
        _containerClient = _blobClient.GetBlobContainerClient(containerName);
        _context = context;
        _logger = logger;
    }

    public async Task<bool> UploadImageAsync(string fileName, string alt, Stream stream)
    {
        ImageEntity image = new() { Path = fileName, Alt = alt, Portfolios = [], Products = [] };
        if (stream != null)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            await _containerClient.CreateIfNotExistsAsync();
            var blob = _containerClient.GetBlobClient(image.Id.ToString());
            await blob.UploadAsync(stream, true);

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<string> GetFileName(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image != null)
        {
            return image.Path;
        }
        return "";
    }

    public async Task<List<ImageDto>> ListImagesAsync()
    {
        return await _context.Images
            .Select(i => MapToImageDto(i))
            .ToListAsync();
    }

    private static ImageDto MapToImageDto(ImageEntity image)
    {
        return new()
        {
            Id = image.Id,
            Path = image.Path,
            Alt = image.Alt
        };
    }

    public async Task<Stream?> GetImageAsync(int id)
    {
        if (!await _context.Images.AnyAsync(i => i.Id == id))
        {
            _logger.LogWarning($"Image {id} not found in the database.");
            return null;
        }

        try
        {
            var blob = _containerClient.GetBlobClient(id.ToString());
            var response = await blob.DownloadAsync();
            return response.Value.Content;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, $"Error getting image {id} from blob storage.");
            return null;
        }
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image != null)
        {
            // delete the image in the database
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            // delete the image in the blob storage
            var blob = _containerClient.GetBlobClient(id.ToString());
            await blob.DeleteIfExistsAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> UpdateImageDescriptionAsync(int id, string alt)
    {
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
}
