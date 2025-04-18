using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.Pages;

public partial class AddProducts
{
    private string ProductName { get; set; } = "";
    private decimal Price { get; set; } = 0;
    private int Quantity { get; set; } = 0;
    private string Description { get; set; } = "";
    private int productTypeId { get; set; }
    private readonly IImageClient _imageClient;
    List<ImageDto>? _images = [];
    List<int> imageIds = [];
    private BlazorProductClient _productClient { get; }
    private string errorString = string.Empty;

    public AddProducts(IImageClient imageClient, BlazorProductClient productClient)
    {
        _productClient = productClient;
        _imageClient = imageClient;
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

    protected override async Task OnInitializedAsync()
    {
        await LoadImages();
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
        return !string.IsNullOrWhiteSpace(ProductName) &&
            Price > 0 &&
            !string.IsNullOrWhiteSpace(Description) &&
            _images != null &&
            _images.Count != 0;
    }

    private async Task AddProduct()
    {
        if (!IsSubmitButtonEnabled()) return;
        var product = new ProductDto
        {
            Name = ProductName,
            Price = Price,
            Quantity = Quantity,
            Description = Description,
            ProductTypeId = productTypeId,
            ImageIds = imageIds
        };
        bool status = await _productClient.CreateProductAsync(product);

        if (status)
        {
            errorString = string.Empty;
        }
        else
        {
            errorString = "Failed to create product.";
        }
    }
}
