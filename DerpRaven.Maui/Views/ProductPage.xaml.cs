using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;
using Microsoft.Maui.Controls;

namespace DerpRaven.Maui.Views;

public partial class ProductPage : ContentPage
{
    ProductPageViewModel _vm;
    public ProductPage(ProductPageViewModel vm)
    {
        _vm = vm;
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        await _vm.RefreshSingleProductView();
    }
}
