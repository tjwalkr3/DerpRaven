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
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _vm.RefreshSingleProductView();
    }
}
