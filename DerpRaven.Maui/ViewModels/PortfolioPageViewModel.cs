using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;
using System.Diagnostics;
namespace DerpRaven.Maui.ViewModels;

[QueryProperty(nameof(SelectedTab), "tab")]
public partial class PortfolioPageViewModel : ObservableObject
{
    public string SelectedTab { get; set; } = "";
    public ObservableCollection<CarouselViewModel> PlushiePortfolios { get; private set; } = [];
    public ObservableCollection<CarouselViewModel> ArtPortfolios { get; private set; } = [];
    private readonly IPortfolioClient _portfolioClient;
    private readonly IImageHelpers _imageHelpers;

    [ObservableProperty]
    private bool isLoading;

    public PortfolioPageViewModel(IPortfolioClient portfolioClient, IImageHelpers imageHelpers)
    {
        _portfolioClient = portfolioClient;
        _imageHelpers = imageHelpers;
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

    private void PopulatePortfolioViews(List<PortfolioDto> portfolios, List<ImageDto> images)
    {
        PlushiePortfolios.Clear();
        ArtPortfolios.Clear();
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
        OnPropertyChanged(nameof(PlushiePortfolios));
        OnPropertyChanged(nameof(ArtPortfolios));
    }

    private async Task<List<ImageDto>> GetPortfolioImages(List<PortfolioDto> portfolios)
    {
        List<int> imageIds = portfolios
            .SelectMany(p => p.ImageIds)
            .Distinct()
            .ToList();

        List<ImageDto> images = await _imageHelpers.GetImageDtos(imageIds);
        images = _imageHelpers.GetPaths(images);
        await _imageHelpers.SaveListOfImages(images);

        return images;
    }
    public void SelectTab()
    {
        Debug.WriteLine($"SelectedTab: {SelectedTab}");
        if (SelectedTab == "Plushie")
        {
            // Logic to select the PlushieTab
            var plushieTab = Shell.Current.Items.FirstOrDefault(i => i.Title == "Plushie Portfolio");
            if (plushieTab != null)
            {
                Shell.Current.CurrentItem = plushieTab;
            }
        }
        else if (SelectedTab == "Art")
        {
            // Logic to select the ArtTab
            var artTab = Shell.Current.Items.FirstOrDefault(i => i.Title == "Art Portfolio");
            if (artTab != null)
            {
                Shell.Current.CurrentItem = artTab;
            }
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
