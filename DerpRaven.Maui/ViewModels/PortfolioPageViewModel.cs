using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;
namespace DerpRaven.Maui.ViewModels;


public partial class PortfolioPageViewModel : ObservableObject
{
    public string SelectedTab { get; set; } = string.Empty;

    public ObservableCollection<CarouselViewModel> PlushiePortfolios { get; private set; } = [];
    public ObservableCollection<CarouselViewModel> ArtPortfolios { get; private set; } = [];
    private readonly IPortfolioClient _portfolioClient;
    private readonly IImageClient _imageClient;

    [ObservableProperty]
    private bool isLoading;

    public PortfolioPageViewModel(IPortfolioClient portfolioClient, IImageClient imageClient)
    {
        _portfolioClient = portfolioClient;
        _imageClient = imageClient;
    }

    public async Task RefreshPortfolioView()
    {
        IsLoading = true;
        try
        {
            List<PortfolioDto> portfolios = await _portfolioClient.GetAllPortfoliosAsync();
            List<ImageDto> images = await GetPortfolioImages(portfolios);
            PopulatePortfolioViews(portfolios, images);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void PopulatePortfolioViews(List<PortfolioDto> portfolios, List<ImageDto> images)
    {
        PlushiePortfolios.Clear();
        ArtPortfolios.Clear();
        MakeCarouselView(portfolios, images);
        OnPropertyChanged(nameof(PlushiePortfolios));
        OnPropertyChanged(nameof(ArtPortfolios));
    }

    public void MakeCarouselView(List<PortfolioDto> portfolios, List<ImageDto> images)
    {
        foreach (var portfolio in portfolios)
        {
            List<ImageDto> portfolioImages = images.Where(img => portfolio.ImageIds.Contains(img.Id)).ToList();
            if (portfolio.ProductTypeId == 1)
            {
                PlushiePortfolios.Add(new CarouselViewModel(portfolio, portfolioImages));
            }
            else if (portfolio.ProductTypeId == 2)
            {
                ArtPortfolios.Add(new CarouselViewModel(portfolio, portfolioImages));
            }
        }
    }

    public async Task<List<ImageDto>> GetImageDtos(List<int> imageIds) => await _imageClient.GetImageInfoManyAsync(imageIds);

    public async Task<List<ImageDto>> GetPortfolioImages(List<PortfolioDto> portfolios)
    {
        List<int> imageIds = portfolios
            .SelectMany(p => p.ImageIds)
            .Distinct()
            .ToList();

        List<ImageDto> images = await GetImageDtos(imageIds);

        return images;
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
