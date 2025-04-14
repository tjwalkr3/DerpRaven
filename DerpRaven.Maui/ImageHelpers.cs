using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui;

public class ImageHelpers(IImageClient imageClient) : IImageHelpers
{
    public List<ImageDto> GetPaths(List<ImageDto> images)
    {
        foreach (var image in images)
        {
            image.Path = Path.Combine(FileSystem.CacheDirectory, $"{image.Id}.png");
        }
        return images;
    }

    // download all images from the server and save them to the device cache
    public async Task SaveListOfImages(List<ImageDto> imageDtos)
    {
        foreach (ImageDto imageDto in imageDtos)
        {
            var image = await imageClient.GetImageAsync(imageDto.Id);
            if (image != null && image.Length != 0)
            {
                SaveImage(image, imageDto.Path);
            }
        }
    }

    // save the downloaded byte array to the device cache, return if it already exists
    public void SaveImage(byte[] image, string path)
    {
        if (File.Exists(path)) return;
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            stream.Write(image, 0, image.Length);
        }
    }

    public async Task<List<ImageDto>> GetImageDtos(List<int> imageIds) => await imageClient.GetImageInfoManyAsync(imageIds);
}
