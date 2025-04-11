using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class CustomRequestPage : Shell
{
    public CustomRequestPage()
    {
        InitializeComponent();

        Navigated += OnNavigated;
    }
    private void OnNavigated(object sender, ShellNavigatedEventArgs e)
    {
        UpdateTitle(); // Update the title every time navigation happens
    }

    private void UpdateTitle()
    {
        if (this.CurrentItem is ShellItem shellItem)
        {
            var activeSection = shellItem.CurrentItem; // Get the active ShellSection
            this.Title = activeSection.Title; // This updates the parent Shell title
        }
    }
}