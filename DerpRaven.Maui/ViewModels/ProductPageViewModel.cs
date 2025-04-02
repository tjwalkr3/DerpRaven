using CommunityToolkit.Mvvm.ComponentModel;
namespace DerpRaven.Maui.ViewModels;


[QueryProperty(nameof(ProductId), "productId")]
public partial class ProductPageViewModel : ObservableObject
{

    [ObservableProperty]
    private int _productId;

    partial void OnProductIdChanged(int oldValue, int newValue)
    {
        throw new NotImplementedException();
    }

}
