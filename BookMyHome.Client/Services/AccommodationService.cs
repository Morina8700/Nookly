using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using BookMyHome.Client.Models;

namespace BookMyHome.Client.Services;

public class AccommodationService
{
    private readonly HttpClient _httpClient;

    public AccommodationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AccommodationDto>> GetAllAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<AccommodationDto>>(
                "api/accommodations")
            ?? new List<AccommodationDto>();
    }

    public async Task<AccommodationDto?> GetByIdAsync(Guid id)
    {
        return await _httpClient
            .GetFromJsonAsync<AccommodationDto>(
                $"api/accommodations/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync(
        CreateAccommodationDto accommodation)
    {
        return await _httpClient.PostAsJsonAsync(
            "api/accommodations",
            accommodation);
    }

    public async Task<HttpResponseMessage> UploadImagesAsync(
    Guid accommodationId,
    IReadOnlyList<IBrowserFile> files)
{
    using var formData = new MultipartFormDataContent();

    foreach (var file in files)
    {
        var stream = file.OpenReadStream(
            maxAllowedSize: 10 * 1024 * 1024);

        var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        formData.Add(
            fileContent,
            "Files",
            file.Name);
    }

    return await _httpClient.PostAsync(
        $"api/accommodations/{accommodationId}/images/upload",
        formData);
}

    public async Task<HttpResponseMessage> UpdateAsync(
        Guid id,
        UpdateAccommodationDto accommodation)
    {
        return await _httpClient.PutAsJsonAsync(
            $"api/accommodations/{id}",
            accommodation);
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync(
            $"api/accommodations/{id}");

        response.EnsureSuccessStatusCode();
    }
}