using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using DerpRaven.Shared.ApiClients;
namespace DerpRaven.Maui.ViewModels;

public partial class PortfolioPageViewModel : ObservableObject
{
    public ObservableCollection<CarouselViewModel> Portfolios { get; private set; }
    private List<ImageDto> Images { get; set; }
    private readonly PortfolioClient _portfolioClient;

    public PortfolioPageViewModel(PortfolioClient client)
    {
        _portfolioClient = client;
        List<PortfolioDto> portfolios = client.GetAllPortfoliosAsync().Result;
        List<ImageDto> images = [];


        Images = new List<ImageDto>
        {
            new ImageDto { Id = 1, Alt = "Derp Squid", Path = "derpsquid.png" },
            new ImageDto { Id = 2, Path = "horsesnuggler.png", Alt = "Horse Snuggler" },
            new ImageDto { Id = 3, Path = "puffersquish.png", Alt = "Puffer Squishy" }
        };

        var portfolioDtos = new List<PortfolioDto>
        {
            new PortfolioDto { Id = 1, Description = "Plushies", Name = "Plush Portfolio", ProductTypeId = 1, ImageIds = new List<int> { 1, 2, 3 } },
            new PortfolioDto { Id = 2, Description = "Art", Name = "Art", ProductTypeId = 1, ImageIds = new List<int> { 3 } }
        };

        Portfolios = new ObservableCollection<CarouselViewModel>(
            portfolioDtos.Select(p => new CarouselViewModel(p, Images))
        );
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



