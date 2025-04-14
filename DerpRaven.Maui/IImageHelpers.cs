using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui
{
    public interface IImageHelpers
    {
        Task<List<ImageDto>> GetImageDtos(List<int> imageIds);
        List<ImageDto> GetPaths(List<ImageDto> images);
        void SaveImage(byte[] image, string path);
        Task SaveListOfImages(List<ImageDto> imageDtos);
    }
}