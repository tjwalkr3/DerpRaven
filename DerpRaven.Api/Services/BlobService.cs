using Azure;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace DerpRaven.Api.Services;

public class BlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IOptions<BlobStorageOptions> options)
    {
        string connectionString = options.Value.ConnectionString
            ?? throw new ArgumentNullException("BlobStorage connection string is missing!");
        string containerName = options.Value.ContainerName;
        BlobServiceClient _blobClient = new BlobServiceClient(connectionString);
        _containerClient = _blobClient.GetBlobContainerClient(containerName);
    }

    public async Task<Azure.Storage.Blobs.Models.BlobContainerInfo> CreateIfNotExistsAsync()
    {
        return await _containerClient.CreateIfNotExistsAsync();
    }

    public async Task<Azure.Storage.Blobs.Models.BlobContentInfo> UploadAsync(string blobName, Stream stream)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.UploadAsync(stream, true);
    }

    public async Task<Azure.Storage.Blobs.Models.BlobDownloadInfo> DownloadAsync(string blobName)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.DownloadAsync();
    }

    public async Task<Response<bool>> DeleteAsync(string blobName)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        return await blob.DeleteIfExistsAsync();
    }
}
