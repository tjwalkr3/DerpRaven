﻿namespace DerpRaven.Web.Pages;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;

public partial class Images
{
    private readonly IImageClient _imageClient;
    List<ImageDto>? _images = [];
    private IBrowserFile? selectedFile;
    private string altText = string.Empty;
    private bool featureFlagEnabled;

    public Images(IImageClient imageClient, IConfiguration config)
    {
        _imageClient = imageClient;
        if (string.IsNullOrEmpty(config["FeatureFlagEnabled"])) {
            featureFlagEnabled = false;
        }
        else
        {
            featureFlagEnabled = bool.Parse(config["FeatureFlagEnabled"] ?? "false");
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