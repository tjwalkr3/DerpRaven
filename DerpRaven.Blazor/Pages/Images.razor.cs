using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
namespace DerpRaven.Blazor.Pages;

public partial class Images
{
    private readonly IBlazorImageClient _imageClient;
    List<ImageDto>? _images = [];
    private IBrowserFile? selectedFile;
    private string altText = string.Empty;
    private string featureFlag;
    private string errorString = string.Empty;

    public Images(IBlazorImageClient imageClient, IConfiguration config)
    {
        _imageClient = imageClient;
        if (string.IsNullOrEmpty(config["FeatureFlagEnabled"]))
        {
            featureFlag = string.Empty;
        }
        else
        {
            featureFlag = config["FeatureFlagEnabled"] ?? string.Empty;
        }
    }

    public bool IsSubmitEnabled() => !string.IsNullOrWhiteSpace(altText) && selectedFile != null;

    protected override async Task OnInitializedAsync()
    {
        await LoadImages();
    }

    public async Task LoadImages()
    {
        try
        {
            _images = await _imageClient.ListImagesAsync();
            foreach (var image in _images)
            {
                image.ImageDataUrl = $"https://derpravenstorage.blob.core.windows.net/images/{image.Id}";
            }
            StateHasChanged();
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
    }

    private void OnFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }

    public async Task AddImage()
    {
        if (IsSubmitEnabled())
        {
            bool result = await _imageClient.UploadImageAsync(selectedFile!, altText);
            if (result)
            {
                altText = string.Empty;
                await LoadImages();
            }
        }
    }

    public async Task DeleteImage(int id)
    {
        bool result = await _imageClient.DeleteImageAsync(id);
        if (result)
        {
            await LoadImages();
        }
    }
}