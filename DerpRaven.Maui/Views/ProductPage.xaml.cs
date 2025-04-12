using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui.Views;

public partial class ProductPage : ContentPage
{
    ProductPageViewModel _viewModel;

    public ProductPage(ProductPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.RefreshSingleProductView();
        }
    }
}
