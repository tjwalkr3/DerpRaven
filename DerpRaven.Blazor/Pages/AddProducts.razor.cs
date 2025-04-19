using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components;

namespace DerpRaven.Blazor.Pages;

public partial class AddProducts
{
    private string ProductName { get; set; } = "";
    private decimal Price { get; set; } = 0m;
    private int Quantity { get; set; } = 0;
    private string Description { get; set; } = "";
    private int ProductTypeId { get; set; }
    private int ProductId { get; set; }

    private readonly IImageClient _imageClient;
    private List<ImageDto>? _images = [];
    private List<int> imageIds = [];
    private BlazorProductClient _productClient { get; }
    private List<ProductDto> _products = [];
    private string errorString = string.Empty;
    private string isEditing = "hidden";

    public AddProducts(IImageClient imageClient, BlazorProductClient productClient)
    {
        _productClient = productClient;
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
        ProductName = "";
        Price = 0m;
        Quantity = 0;
        Description = "";
        ProductTypeId = 0;
        ProductId = 0;
    }

    public void OnProductChanged(ChangeEventArgs e)
    {
        if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString())) return;
        int productId = int.Parse(e.Value.ToString()!);
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null) return;

        ProductName = product.Name;
        Price = product.Price;
        Quantity = product.Quantity;
        Description = product.Description;
        ProductTypeId = product.ProductTypeId;
        imageIds = product.ImageIds;
        ProductId = product.Id;
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

    private async Task LoadProducts()
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
        await LoadImages();
        await LoadProducts();
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

    public async Task AddProduct()
    {
        if (!IsSubmitButtonEnabled()) return;
        var product = new ProductDto
        {
            Name = ProductName,
            Price = Price,
            Quantity = Quantity,
            Description = Description,
            ProductTypeId = ProductTypeId,
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

    public async Task UpdateProduct()
    {
        if (!IsSubmitButtonEnabled()) return;
        var product = new ProductDto
        {
            Name = ProductName,
            Price = Price,
            Quantity = Quantity,
            Description = Description,
            ProductTypeId = ProductTypeId,
            ImageIds = imageIds,
            Id = ProductId
        };
        bool status = await _productClient.UpdateProductAsync(product);
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
