using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.Blazor.Pages;

public partial class Portfolio
{
    private List<PortfolioDto>? _portfolios = [];
    private string errorString = string.Empty;
    BlazorPortfolioClient _portfolioClient { get; }

    public Portfolio(BlazorPortfolioClient portfolioClient)
    {
        _portfolioClient = portfolioClient;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadPortfolios();
    }

    private async Task LoadPortfolios()
    {
        try
        {
            _portfolios = await _portfolioClient.GetAllPortfoliosAsync();
            errorString = string.Empty;
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
    }
}

