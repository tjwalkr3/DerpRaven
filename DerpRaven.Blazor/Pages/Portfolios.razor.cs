using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components;
namespace DerpRaven.Blazor.Pages;

public partial class Portfolios
{
    private string PortfolioName { get; set; } = "";
    private string Description { get; set; } = "";
    private int ProductTypeId { get; set; }
    private int PortfolioId { get; set; }

    private readonly IBlazorImageClient _imageClient;
    private List<ImageDto>? _images = [];
    private List<int> imageIds = [];
    private BlazorPortfolioClient _portfolioClient { get; }
    private List<PortfolioDto> _portfolios = [];
    private string errorString = string.Empty;
    private string isEditing = "hidden";

    public Portfolios(IBlazorImageClient imageClient, BlazorPortfolioClient portfolioClient)
    {
        _portfolioClient = portfolioClient;
        _imageClient = imageClient;

    }

    public async Task IsEditingChanged()
    {
        if (isEditing == "visible")
        {
            isEditing = "hidden";
            ClearFields();
            await InvokeAsync(StateHasChanged);
        }
        else
        {
            isEditing = "visible";
            await InvokeAsync(StateHasChanged);
        }
    }

    public void ClearFields()
    {
        PortfolioName = "";
        Description = "";
        ProductTypeId = 0;
        PortfolioId = 0;
    }

    public void OnPortfolioChanged(ChangeEventArgs e)
    {
        if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString())) return;
        int portfolioId = int.Parse(e.Value.ToString()!);
        var product = _portfolios.FirstOrDefault(p => p.Id == portfolioId);
        if (product == null) return;

        PortfolioName = product.Name;
        Description = product.Description;
        ProductTypeId = product.ProductTypeId;
        imageIds = product.ImageIds;
        PortfolioId = product.Id;
    }

    public async Task LoadImages()
    {
        try
        {
            _images = await _imageClient.ListImagesAsync();
            foreach (var image in _images)
            {
                var imageData = await _imageClient.GetImageAsync(image.Id);
                if (imageData != null)
                {
                    image.ImageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(imageData)}";
                }
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
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

    protected override async Task OnInitializedAsync()
    {
        await LoadImages();
        await LoadPortfolios();
    }

    public void SelectImage(int imageId)
    {
        if (imageIds.Contains(imageId))
        {
            imageIds.Remove(imageId);
        }
        else
        {
            imageIds.Add(imageId);
        }
    }

    public bool IsSubmitButtonEnabled()
    {
        return !string.IsNullOrWhiteSpace(PortfolioName) &&
            !string.IsNullOrWhiteSpace(Description) &&
            _images != null &&
            _images.Count != 0;
    }

    public async Task AddPortfolio()
    {
        if (!IsSubmitButtonEnabled()) return;
        var portfolio = new PortfolioDto
        {
            Name = PortfolioName,
            Description = Description,
            ProductTypeId = ProductTypeId,
            ImageIds = imageIds
        };
        bool status = await _portfolioClient.CreatePortfolioAsync(portfolio);

        if (status)
        {
            errorString = string.Empty;
        }
        else
        {
            errorString = "Failed to create product.";
        }
    }

    public async Task UpdatePortfolio()
    {
        if (!IsSubmitButtonEnabled()) return;
        var portfolio = new PortfolioDto
        {
            Name = PortfolioName,
            Description = Description,
            ProductTypeId = ProductTypeId,
            ImageIds = imageIds,
            Id = PortfolioId
        };
        bool status = await _portfolioClient.UpdatePortfolioAsync(portfolio);
        if (status)
        {
            errorString = string.Empty;
        }
        else
        {
            errorString = "Failed to update product.";
        }
    }
}
