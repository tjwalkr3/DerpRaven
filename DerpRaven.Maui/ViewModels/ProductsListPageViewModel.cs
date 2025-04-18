﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DerpRaven.Maui.ViewModels;

public partial class ProductsListPageViewModel : ObservableObject
{
    public ObservableCollection<ProductDto> PlushieProducts { get; private set; } = [];
    public ObservableCollection<ProductDto> ArtProducts { get; private set; } = [];
    private readonly IProductClient _productClient;
    private readonly IImageHelpers _imageHelpers;

    [ObservableProperty]
    private bool isLoading;

    public ProductsListPageViewModel(IProductClient productClient, IImageHelpers imageHelpers)
    {
        _productClient = productClient;
        _imageHelpers = imageHelpers;
    }

    public async Task RefreshProductsView()
    {
        IsLoading = true;
        try
        {
            List<ProductDto> products = await _productClient.GetAllProductsAsync();
            List<ImageDto> images = await DownloadAllImages(products);
            PopulateProductViews(products, images);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void PopulateProductViews(List<ProductDto> products, List<ImageDto> images)
    {
        PlushieProducts.Clear();
        ArtProducts.Clear();

        foreach (var product in products)
        {
            product.ImagePath = images.FirstOrDefault(img => img.Id == product.ImageIds[0])?.Path; // set the path to the main image
            if (product.ProductTypeId == 1)
            {
                PlushieProducts.Add(product);
            }
            else if (product.ProductTypeId == 2)
            {
                ArtProducts.Add(product);
            }
        }
    }

    public async Task<List<ImageDto>> DownloadAllImages(List<ProductDto> products)
    {
        List<int> imageIds = [];

        foreach (var product in products)
        {
            if (product.ImageIds != null && product.ImageIds.Count > 0)
            {
                imageIds.Add(product.ImageIds[0]);
            }
        }
        imageIds = imageIds.Distinct().ToList();

        List<ImageDto> images = await _imageHelpers.GetImageDtos(imageIds);
        images = _imageHelpers.GetPaths(images);
        await _imageHelpers.SaveListOfImages(images);

        return images;
    }

    [RelayCommand]
    private void AddToCart(ProductDto product)
    {
        if (product == null) return;
        Debug.WriteLine($"Added {product.Name} to cart!");
        // Here you could add logic to update a cart collection
    }

    [RelayCommand]
    private async Task NavigateToProduct(ProductDto product)
    {

        if (product == null) return;
        await Shell.Current.GoToAsync($"ProductPage?productId={product.Id}");
    }
}