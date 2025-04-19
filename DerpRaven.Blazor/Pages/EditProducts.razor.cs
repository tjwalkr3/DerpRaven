using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.Pages;

partial class EditProducts
{
    private string errorString = string.Empty;
    private readonly BlazorProductClient _productClient;
    private List<ProductDto> _products = [];

    public EditProducts(BlazorProductClient productClient)
    {
        _productClient = productClient;
    }

    private async Task LoadRequests()
    {
        try
        {
            _products = await _productClient.GetAllProductsAsync();
            errorString = string.Empty;
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadRequests();
    }
}
