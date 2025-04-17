﻿using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace DerpRaven.Blazor.ApiClients;

public class BlazorImageClient : IImageClient
{
    private readonly HttpClient _httpClient;

    public BlazorImageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/image/delete/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<byte[]?> GetImageAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/image/get/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        return null;
    }

    public async Task<ImageDto?> GetImageInfoAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ImageDto>($"api/image/info/{id}");
        return response;
    }

    public async Task<List<ImageDto>> GetImageInfoManyAsync(List<int> ids)
    {
        string path = $"api/image/info-many/?{string.Join("&", ids.Select(id => $"ids={id}"))}";
        var response = await _httpClient.GetFromJsonAsync<List<ImageDto>>(path);
        return response ?? [];
    }

    public async Task<List<ImageDto>> ListImagesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<ImageDto>>("api/image/list");
        return response ?? [];
    }

    public async Task<bool> UploadImageAsync(IBrowserFile file, string description)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream(10_000_000)), "File", file.Name }, // 10 MB limit
            { new StringContent(description), "Description" }
        };

        var response = await _httpClient.PostAsync("api/image/upload", content);
        return response.IsSuccessStatusCode;
    }
}
