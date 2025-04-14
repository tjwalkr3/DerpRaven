using Azure;
using Azure.Storage.Blobs.Models;

namespace DerpRaven.Api.Services
{
    public interface IBlobService
    {
        Task CreateIfNotExistsAsync();
        Task<Response<bool>> DeleteAsync(string blobName);
        Task<Stream> DownloadAsync(string blobName);
        Task<BlobContentInfo> UploadAsync(string blobName, Stream stream);
    }
}