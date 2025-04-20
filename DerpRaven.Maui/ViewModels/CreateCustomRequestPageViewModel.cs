using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;

public partial class CreateCustomRequestPageViewModel(ICustomRequestClient client, ILogger<CreateCustomRequestPageViewModel> logger) : ObservableObject
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
            //TODO: Navigate to view custom requests page
            //TODO: Clear the form after submission
        } catch (Exception ex)
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
