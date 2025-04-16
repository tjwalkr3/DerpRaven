using DerpRaven.Maui.ViewModels;
using System.Threading.Tasks;
namespace DerpRaven.Maui.Views;

public partial class CustomRequestPage : Shell
{
    public CustomRequestPage()
    {
        InitializeComponent();

        Navigated += OnNavigated;
    }
    private async void OnNavigated(object sender, ShellNavigatedEventArgs e)
    {
        UpdateTitle(); // Update the title every time navigation happens
    }

    private async Task UpdateTitle()
    {
        if (this.CurrentItem is ShellItem shellItem)
        {
            var activeSection = shellItem.CurrentItem; // Get the active ShellSection
            this.Title = activeSection.Title; // This updates the parent Shell title
            if (this.Title == "View")
            {
                if (this.CurrentPage is Page page &&
                    page.BindingContext is ViewCustomRequestsPageViewModel vm)
                {
                    await vm.GetCustomRequests();
                }
            }
        }
    }
}