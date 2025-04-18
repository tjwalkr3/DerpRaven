using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
namespace DerpRaven.Blazor.Pages;

public partial class Images
{
    private readonly BlazorImageClient _imageClient;
    List<ImageDto>? _images = [];
    private IBrowserFile? selectedFile;
    private string altText = string.Empty;
    private string featureFlag;
    private string errorString = string.Empty;
    private readonly IConfiguration _config;

    public Images(BlazorImageClient imageClient, IConfiguration config)
    {
        _imageClient = imageClient;
        _config = config;
        if (string.IsNullOrEmpty(config["FeatureFlagEnabled"]))
        {
            featureFlag = string.Empty;
        }
        else
        {
            featureFlag = config["FeatureFlagEnabled"] ?? string.Empty;
        }
    }

    public string IsSubmitButtonEnabled()
    {
        if (string.IsNullOrWhiteSpace(altText) || selectedFile == null) return "disabled";
        return string.Empty;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadImages();
    }

    public async Task LoadImages()
    {
        try
        {
            Console.WriteLine("We are trying to load images");
            _images = await _imageClient.ListImagesAsync();
            foreach (var image in _images)
            {
                Console.WriteLine("Getting first uri");
                image.ImageDataUrl = _imageClient.GetImageAddress(image.Id);
                Console.WriteLine($"Image ID: {image.Id}, URL: {image.ImageDataUrl}");
            }
            await InvokeAsync(StateHasChanged);
            Console.WriteLine("We should have all the images now"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine("We failed the load");
            errorString = ex.Message;
        }
    }

    private void OnFileSelected(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }

    public async Task AddImage()
    {
        if (selectedFile != null)
        {
            bool result = await _imageClient.UploadImageAsync(selectedFile, altText);
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