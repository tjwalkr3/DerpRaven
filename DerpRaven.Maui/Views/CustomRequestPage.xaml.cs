using DerpRaven.Maui.ViewModels;
using System.Threading.Tasks;
using static DerpRaven.Maui.ViewModels.CreateCustomRequestPageViewModel;
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Check if the NavigationState.SelectedTab is set
        if (!string.IsNullOrEmpty(NavigationState.ViewTab))
        {
            var tabBar = this.Items.FirstOrDefault() as TabBar;

            var route = NavigationState.ViewTab;

            ShellSection? targetTab = null;

            if (NavigationState.ViewTab == "ViewCustomRequestPage")
            {
                targetTab = tabBar?.Items.FirstOrDefault(i => i.Route == route);
            }

            if (targetTab != null)
            {
                tabBar.CurrentItem = targetTab;
            }

            NavigationState.ViewTab = null; // Clear it after use
        }
    }

}