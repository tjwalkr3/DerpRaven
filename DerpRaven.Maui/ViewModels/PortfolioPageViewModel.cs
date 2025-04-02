using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.Maui.ViewModels;



public partial class PortfolioPageViewModel : ObservableObject {
    public ObservableCollection<PortfolioViewModel> Portfolios { get; private set; }
    private List<ImageDto> Images { get; set; }

    public PortfolioPageViewModel() {
        Images = new List<ImageDto>
        {
        new ImageDto { Id = 1, Alt = "Derp Squid", Path = "derpsquid.png" },
        new ImageDto { Id = 2, Path = "horsesnuggler.png", Alt = "Horse Snuggler" },
        new ImageDto { Id = 3, Path = "puffersquish.png", Alt = "Puffer Squishy" }
    };

        var portfolioDtos = new List<PortfolioDto>
        {
        new PortfolioDto { Id = 1, Description = "Plushies", Name = "Plush Portfolio", ProductTypeId = 1, ImageIds = new List<int> { 1, 2, 3 } },
        new PortfolioDto { Id = 2, Description = "Plushies2", Name = "Plush Portfolio2", ProductTypeId = 1, ImageIds = new List<int> { 3 } }
    };

        Portfolios = new ObservableCollection<PortfolioViewModel>(
            portfolioDtos.Select(p => new PortfolioViewModel(p, Images))
        );
    }
}



public class PortfolioViewModel : ObservableObject {
    public PortfolioDto Portfolio { get; }
    public ObservableCollection<ImageDto> Images { get; }

    public PortfolioViewModel(PortfolioDto portfolio, List<ImageDto> allImages) {
        Portfolio = portfolio;
        Images = new ObservableCollection<ImageDto>(
            allImages.Where(img => portfolio.ImageIds.Contains(img.Id))
        );
    }
}



