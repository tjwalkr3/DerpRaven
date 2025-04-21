using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.Blazor.Pages;

public partial class CustomRequests
{
    private List<CustomRequestDto>? _requests = [];
    private string errorString = string.Empty;
    IBlazorCustomRequestClient _customRequestClient { get; }

    public CustomRequests(IBlazorCustomRequestClient customRequestClient)
    {
        _customRequestClient = customRequestClient;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadRequests();
    }

    private async Task LoadRequests()
    {
        try
        {
            _requests = await _customRequestClient.GetAllCustomRequestsAsync();
            errorString = string.Empty;
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
    }

    private async Task ChangeStatus(int id, string status)
    {
        await _customRequestClient.ChangeStatusAsync(id, status);
        await LoadRequests();
    }
}
