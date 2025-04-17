namespace DerpRaven.Blazor.Pages;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;

public partial class CustomRequests
{
    private List<CustomRequestDto>? _requests = [];
    private string errorString = string.Empty;
    ICustomRequestClient _customRequestClient { get; }

    public CustomRequests(ICustomRequestClient customRequestClient)
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
