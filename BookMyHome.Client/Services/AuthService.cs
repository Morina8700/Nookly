using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookMyHome.Client.Models;
using Microsoft.JSInterop;

namespace BookMyHome.Client.Services;

public class AuthService
{
    private const string TokenStorageKey = "bookmyhome_token";
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public LoginResponse? CurrentUser { get; private set; }
    public event Action? AuthenticationStateChanged;

    public AuthService(
        HttpClient httpClient,
        IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(CurrentUser?.Token);

    public async Task InitializeAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenStorageKey);

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            CurrentUser = new LoginResponse
            {
                Token = token
            };
        }
    }

    public async Task<(bool Success, string? Error)> LoginAsync(
        LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            request);

        if (!response.IsSuccessStatusCode)
        {
            return (false, await response.Content.ReadAsStringAsync());
        }

        var loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginResponse == null
            || string.IsNullOrWhiteSpace(loginResponse.Token))
        {
            return (false, "The login response did not contain a token.");
        }

        CurrentUser = loginResponse;
        await SaveTokenAsync(loginResponse.Token);

        return (true, null);
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            TokenStorageKey);

        AuthenticationStateChanged?.Invoke();
    }

    private async Task SaveTokenAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            TokenStorageKey,
            token);

        AuthenticationStateChanged?.Invoke();
    }
}
