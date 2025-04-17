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
        await GetRequests(); // Update the title every time navigation happens
    }

    private async Task GetRequests()
    {
        if (this.CurrentItem is ShellItem shellItem)
        {
            var activeSection = shellItem.CurrentItem; // Get the active ShellSection
            if (activeSection?.Title == "View")
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