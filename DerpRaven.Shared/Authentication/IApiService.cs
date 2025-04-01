
namespace DerpRaven.Shared.Authentication
{
    public interface IApiService
    {
        Task<HttpResponseMessage> DeleteAsync(string endpoint);
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<HttpResponseMessage> GetAsyncWithoutAuthorization(string endpoint);
        Task<T?> GetFromJsonAsyncWithoutAuthorization<T>(string endpoint);
        Task<HttpResponseMessage> PatchAsync(string endpoint, HttpContent content);
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string endpoint, T content);
        Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content);
        Task<HttpResponseMessage> PutAsJsonAsync<T>(string endpoint, T content);
    }
}