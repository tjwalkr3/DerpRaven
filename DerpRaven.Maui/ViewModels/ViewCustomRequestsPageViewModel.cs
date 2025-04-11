using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerpRaven.Maui.ViewModels;

public partial class ViewCustomRequestsPageViewModel(ICustomRequestClient client, ILogger<CustomRequestPageViewModel> logger) : ObservableObject
{
    public ObservableCollection<CustomRequestDto> CustomRequests { get; set; } = [];

    [RelayCommand]
    public async Task GetCustomRequests()
    {
        try
        {
            // See if we can get all custom requests
            List<CustomRequestDto>? requests = await client.GetCustomRequestsByUserEmailAsync();

            // If we don't find any, we don't need to do much, just return, leaving the list empty
            if (requests == null)
            {
                logger.LogError("No custom requests found");
                return;
            }
            ;

            // If we do find some, we need to set the list to include the requests
            logger.LogInformation("Found {Count} custom requests", requests.Count);
            CustomRequests.Clear();
            foreach (var request in requests)
            {
                CustomRequests.Add(request);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting custom requests");
            throw;
        }
    }
}
