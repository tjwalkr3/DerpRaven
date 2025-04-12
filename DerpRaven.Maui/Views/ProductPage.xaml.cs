using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui.Views;

public partial class ProductPage : ContentPage
{
    

    public ProductPage(ProductPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        
    }

    
}
