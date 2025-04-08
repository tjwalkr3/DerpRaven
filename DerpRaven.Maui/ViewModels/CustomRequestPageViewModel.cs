using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;
namespace DerpRaven.Maui.ViewModels;

public partial class CustomRequestPageViewModel(CustomRequestClient client) : ObservableObject
{
    private ObservableCollection<CustomRequestDto> CustomRequests;

    [RelayCommand]
    public async Task GetCustomRequests()
    {
        // See if we can get all custom requests
        List<CustomRequestDto>? requests = await client.GetCustomRequestsByUserEmailAsync();
        // If we don't find any, we don't need to do much, just return, leaving the list empty
        if (requests == null) return;
        // If we do find some, we need to set the list to include the requests
        CustomRequests = new(requests);
    }
}
