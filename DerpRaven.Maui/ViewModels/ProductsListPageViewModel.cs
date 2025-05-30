﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DerpRaven.Maui.ViewModels;

public partial class ProductsListPageViewModel : ObservableObject
{
    public ObservableCollection<ProductDto> PlushieProducts { get; private set; } = [];
    public ObservableCollection<ProductDto> ArtProducts { get; private set; } = [];
    private readonly IProductClient _productClient;
    private readonly IImageClient _imageClient;

    [ObservableProperty]
    private bool isLoading;

    public ProductsListPageViewModel(IProductClient productClient, IImageClient imageClient)
    {
        _productClient = productClient;
        _imageClient = imageClient;
    }

    public async Task RefreshProductsView()
    {
        IsLoading = true;
        try
        {
            List<ProductDto> products = await _productClient.GetAllProductsAsync();
            products = products.Where(p => p.Quantity > 0).ToList(); // Filter out 0 quantity products
            List<ImageDto> images = await GetAllImageDtos(products);
            PopulateProductViews(products, images);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void PopulateProductViews(List<ProductDto> products, List<ImageDto> images)
    {
        PlushieProducts.Clear();
        ArtProducts.Clear();

        foreach (var product in products)
        {
            product.ImagePath = images.FirstOrDefault(img => img.Id == product.ImageIds[0])?.Id.ToString(); // set the path to the main image's id
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

    public async Task<List<ImageDto>> GetImageDtos(List<int> imageIds) => await _imageClient.GetImageInfoManyAsync(imageIds);

    public async Task<List<ImageDto>> GetAllImageDtos(List<ProductDto> products)
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

        List<ImageDto> images = await GetImageDtos(imageIds);

        return images;
    }

    [RelayCommand]
    private async Task NavigateToProduct(ProductDto product)
    {
        if (product == null) return;
        await Shell.Current.GoToAsync($"ProductPage?productId={product.Id}");
    }
}