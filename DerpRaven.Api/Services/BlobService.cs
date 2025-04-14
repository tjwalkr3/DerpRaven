using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
namespace DerpRaven.Api.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobService> _logger;

    public BlobService(IOptions<BlobStorageOptions> options, ILogger<BlobService> logger)
    {
        string connectionString = options.Value.ConnectionString
            ?? throw new ArgumentNullException("BlobStorage connection string is missing!");
        string containerName = options.Value.ContainerName
            ?? throw new ArgumentNullException("Blobstorage container name is missing!");
        BlobServiceClient _blobClient = new BlobServiceClient(connectionString);
        _containerClient = _blobClient.GetBlobContainerClient(containerName);
        _logger = logger;
    }

    public async Task CreateIfNotExistsAsync()
    {
        _logger.LogInformation("Creating container {ContainerName}", _containerClient.Name);
        await _containerClient.CreateIfNotExistsAsync();
    }

    public async Task<BlobContentInfo> UploadAsync(string blobName, Stream stream)
    {
        _logger.LogInformation("Uploading blob {BlobName} to container {ContainerName}", blobName, _containerClient.Name);
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.UploadAsync(stream, true);
    }

    public async Task<Stream> DownloadAsync(string blobName)
    {
        _logger.LogInformation("Downloading blob {BlobName} from container {ContainerName}", blobName, _containerClient.Name);
        var blob = _containerClient.GetBlobClient(blobName);
        var result = await blob.DownloadAsync();
        return result.Value.Content;
    }

    public async Task<Response<bool>> DeleteAsync(string blobName)
    {
        _logger.LogInformation("Deleting blob {BlobName} from container {ContainerName}", blobName, _containerClient.Name);
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.DeleteIfExistsAsync();
    }
}
