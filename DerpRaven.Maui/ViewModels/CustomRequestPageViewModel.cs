using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
namespace DerpRaven.Maui.ViewModels;

public partial class CustomRequestPageViewModel(ICustomRequestClient client, ILogger<CustomRequestPageViewModel> logger) : ObservableObject
{
    public ObservableCollection<CustomRequestDto> CustomRequests { get; set; } = [];

    [ObservableProperty]
    private bool isArt = false;

    [ObservableProperty]
    private bool isPlushie = false;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string artType = string.Empty;

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
            };

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

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    public async Task Submit()
    {
        try
        {
            CustomRequestDto request = new()
            {
                Description = IsArt ? $"Type: {ArtType} - {Description}" : $"Type: Plushie - {Description}",
                Email = Email,
                Status = "Pending",
                ProductTypeId = IsArt ? 2 : 1
            };
            bool success = await client.CreateCustomRequestAsync(request);
            if (success) await GetCustomRequests();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error submitting custom request");
            throw;
        }
    }

    private bool CanSubmit()
    {
        if (IsArt && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(ArtType)) return true;
        if (IsPlushie && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Email)) return true;
        return false;
    }

    partial void OnIsArtChanged(bool oldValue, bool newValue)
    {
        SubmitCommand.NotifyCanExecuteChanged();
        if (newValue) IsPlushie = false;
    }

    partial void OnIsPlushieChanged(bool oldValue, bool newValue)
    {
        SubmitCommand.NotifyCanExecuteChanged();
        if (newValue) IsArt = false;
    }

    partial void OnEmailChanged(string? oldValue, string newValue)
    {
        SubmitCommand.NotifyCanExecuteChanged();
    }

    partial void OnDescriptionChanged(string? oldValue, string newValue)
    {
        SubmitCommand.NotifyCanExecuteChanged();
    }

    partial void OnArtTypeChanged(string? oldValue, string newValue)
    {
        SubmitCommand.NotifyCanExecuteChanged();
    }
}
