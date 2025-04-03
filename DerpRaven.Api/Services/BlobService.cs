using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
namespace DerpRaven.Api.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IOptions<BlobStorageOptions> options)
    {
        string connectionString = options.Value.ConnectionString
            ?? throw new ArgumentNullException("BlobStorage connection string is missing!");
        string containerName = options.Value.ContainerName
            ?? throw new ArgumentNullException("Blobstorage container name is missing!");
        BlobServiceClient _blobClient = new BlobServiceClient(connectionString);
        _containerClient = _blobClient.GetBlobContainerClient(containerName);
    }

    public async Task<BlobContainerInfo> CreateIfNotExistsAsync()
    {
        return await _containerClient.CreateIfNotExistsAsync();
    }

    public async Task<BlobContentInfo> UploadAsync(string blobName, Stream stream)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.UploadAsync(stream, true);
    }

    public async Task<Stream> DownloadAsync(string blobName)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        var result = await blob.DownloadAsync();
        return result.Value.Content;
    }

    public async Task<Response<bool>> DeleteAsync(string blobName)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.DeleteIfExistsAsync();
    }
}
