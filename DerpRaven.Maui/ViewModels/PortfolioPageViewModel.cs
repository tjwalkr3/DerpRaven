using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;
namespace DerpRaven.Maui.ViewModels;

public partial class PortfolioPageViewModel : ObservableObject
{
    public ObservableCollection<CarouselViewModel> PlushiePortfolios { get; private set; } = [];
    public ObservableCollection<CarouselViewModel> ArtPortfolios { get; private set; } = [];
    private readonly IPortfolioClient _portfolioClient;
    private readonly IImageClient _imageClient;

    public PortfolioPageViewModel(IPortfolioClient portfolioClient, IImageClient imageClient)
    {
        _portfolioClient = portfolioClient;
        _imageClient = imageClient;
    }

    public async Task RefreshPortfolioView()
    {
        List<PortfolioDto> portfolios = await _portfolioClient.GetAllPortfoliosAsync();
        List<ImageDto> images = await GetPortfolioImages(portfolios);
        PopulatePortfolioViews(portfolios, images);
    }

    private void PopulatePortfolioViews(List<PortfolioDto> portfolios, List<ImageDto> images)
    {
        PlushiePortfolios.Clear();
        ArtPortfolios.Clear();
        foreach (var portfolio in portfolios)
        {
            if (portfolio.ProductTypeId == 1)
            {
                PlushiePortfolios.Add(new CarouselViewModel(portfolio, images));
            }
            else if (portfolio.ProductTypeId == 2)
            {
                ArtPortfolios.Add(new CarouselViewModel(portfolio, images));
            }
        }
        OnPropertyChanged(nameof(PlushiePortfolios));
        OnPropertyChanged(nameof(ArtPortfolios));
    }

    private async Task<List<ImageDto>> GetPortfolioImages(List<PortfolioDto> portfolios)
    {
        List<int> imageIds = portfolios
            .SelectMany(p => p.ImageIds)
            .Distinct()
            .ToList();

        List<ImageDto> images = await _imageClient.GetImageInfoManyAsync(imageIds);
        images = GetPaths(images);
        await SaveListOfImages(images);

        return images;
    }

    private List<ImageDto> GetPaths(List<ImageDto> images)
    {
        foreach (var image in images)
        {
            image.Path = Path.Combine(FileSystem.CacheDirectory, $"{image.Id}.png");
        }
        return images;
    }

    // download all images from the server and save them to the device cache
    private async Task SaveListOfImages(List<ImageDto> imageDtos)
    {
        foreach (ImageDto imageDto in imageDtos)
        {
            var image = await _imageClient.GetImageAsync(imageDto.Id);
            if (image != null && image.Length != 0)
            {
                SaveImage(image, imageDto.Path);
            }
        }
    }

    // save the downloaded byte array to the device cache, return if it already exists
    private void SaveImage(byte[] image, string path)
    {
        if (File.Exists(path)) return;
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            stream.Write(image, 0, image.Length);
        }
    }
}

public class CarouselViewModel : ObservableObject
{
    public PortfolioDto Portfolio { get; }
    public ObservableCollection<ImageDto> Images { get; }

    public CarouselViewModel(PortfolioDto portfolio, List<ImageDto> allImages)
    {
        Portfolio = portfolio;
        Images = new ObservableCollection<ImageDto>(
            allImages.Where(img => portfolio.ImageIds.Contains(img.Id))
        );
    }
}

// a test for the pr-kube-test branch
// another test for the pr-kube-test branch
// another test for the pr-kube-test branch
// another test for the pr-kube-test branch
// another test for the pr-kube-test branch