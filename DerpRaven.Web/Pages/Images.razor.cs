namespace DerpRaven.Web.Pages;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

public partial class Images
{
    private readonly IImageClient _imageClient;
    List<ImageDto>? _images = [];
    private IBrowserFile? selectedFile;
    private string description = string.Empty;

    public Images(IImageClient imageClient)
    {
        _imageClient = imageClient;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadImages();
    }

    public async Task LoadImages()
    {
        _images = await _imageClient.ListImagesAsync();
        StateHasChanged();
    }

    private void OnFileSelected(ChangeEventArgs e)
    {
        selectedFile = (e.Value as IBrowserFile);
    }

    public async Task AddImage()
    {
        if (selectedFile != null)
        {
            bool result = await _imageClient.UploadImageAsync(selectedFile, description);
            if (result)
            {
                await LoadImages();
            }
        }
    }
}