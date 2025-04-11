namespace DerpRaven.Web.Pages;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;

public partial class CustomRequests
{
    private List<CustomRequestDto>? _requests = [];
    private CustomRequestDto _newRequest = new();
    private string _status = string.Empty;
    private string _productType = string.Empty;
    private int _id = 0;
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

    private async Task CreateRequest()
    {
        await _customRequestClient.CreateCustomRequestAsync(_newRequest);
        _newRequest = new CustomRequestDto();
        await LoadRequests();
    }

    private async Task GetRequestsByUser()
    {
        _requests = await _customRequestClient.GetCustomRequestsByUserEmailAsync();
    }

    private async Task GetRequestsByStatus()
    {
        _requests = await _customRequestClient.GetCustomRequestsByStatusAsync(_status);
    }

    private async Task GetRequestsByType()
    {
        _requests = await _customRequestClient.GetCustomRequestsByTypeAsync(_productType);
    }

    private async Task ChangeStatus()
    {
        await _customRequestClient.ChangeStatusAsync(_id, _status);
        await LoadRequests();
    }
}
