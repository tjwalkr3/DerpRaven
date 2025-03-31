using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace DerpRaven.Shared.Authentication;

public class ApiService : IApiService
{
    private readonly IKeycloakClient keycloakClient;
    private HttpClient _httpClient;

    public ApiService(IKeycloakClient keycloakClient, IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        this.keycloakClient = keycloakClient;
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloakClient.IdentityToken);

        return await _httpClient.GetAsync(endpoint);
    }

    public async Task<T?> GetFromJsonAsyncWithoutAuthorization<T>(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        var response = await _httpClient.GetFromJsonAsync<T>(endpoint);

        return response;
    }

    public async Task<HttpResponseMessage> GetAsyncWithoutAuthorization(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        var response = await _httpClient.GetAsync(endpoint);

        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloakClient.IdentityToken);

        return await _httpClient.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string endpoint, T content)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloakClient.IdentityToken);

        return await _httpClient.PostAsJsonAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloakClient.IdentityToken);

        return await _httpClient.DeleteAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PatchAsync(string endpoint, HttpContent content)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keycloakClient.IdentityToken);

        return await _httpClient.PatchAsync(endpoint, content);
    }
}