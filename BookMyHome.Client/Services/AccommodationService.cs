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

 public async Task<List<AccommodationDto>> GetAllAsync(
    string? location = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    bool? hasWifi = null,
    int? maxGuests = null)
{
    var query = new List<string>();

    if (!string.IsNullOrWhiteSpace(location))
    {
        query.Add(
            $"location={Uri.EscapeDataString(location)}");
    }

    if (minPrice.HasValue)
    {
        query.Add(
            $"minPrice={minPrice.Value}");
    }

    if (maxPrice.HasValue)
    {
        query.Add(
            $"maxPrice={maxPrice.Value}");
    }

    if (hasWifi.HasValue)
    {
        query.Add(
            $"hasWifi={hasWifi.Value.ToString().ToLowerInvariant()}");
    }

    if (maxGuests.HasValue)
    {
        query.Add(
            $"maxGuests={maxGuests.Value}");
    }

    var url = "api/accommodations";

    if (query.Count > 0)
    {
        url += "?" + string.Join("&", query);
    }

    return await _httpClient
        .GetFromJsonAsync<List<AccommodationDto>>(url)
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