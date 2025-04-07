using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class CustomRequestPage : ContentPage
{
    public CustomRequestPage(CustomRequestPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == ArtCheckBox && e.Value)
        {
            PlushieCheckBox.IsChecked = false;
            ArtOptionsPicker.SelectedIndex = 0;
        }
        else if (sender == PlushieCheckBox && e.Value)
        {
            ArtCheckBox.IsChecked = false;
        }
    }
}