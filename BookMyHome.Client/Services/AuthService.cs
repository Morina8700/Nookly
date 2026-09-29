using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BookMyHome.Client.Models;
using Microsoft.JSInterop;

namespace BookMyHome.Client.Services;

public class AuthService
{
    private const string TokenStorageKey = "bookmyhome_token";
    private const string UserStorageKey = "bookmyhome_user";
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


                var userJson = await _jsRuntime.InvokeAsync<string?>(
                    "localStorage.getItem",
                    UserStorageKey
                );

                if(!string.IsNullOrWhiteSpace(userJson))
                {
                CurrentUser = JsonSerializer.Deserialize<LoginResponse>(userJson);
            }

            CurrentUser ??= new LoginResponse
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
        await SaveTokenAsync(loginResponse);

        return (true, null);
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.removeItem",
            TokenStorageKey);
            
            await _jsRuntime.InvokeVoidAsync(
                "localStorage.removeItem",
                UserStorageKey);

        AuthenticationStateChanged?.Invoke();
    }

    private async Task SaveTokenAsync(LoginResponse loginResponse)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue(
            "Bearer",
            loginResponse.Token);
        
        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            TokenStorageKey,
            loginResponse.Token
        );

        var userJson = JsonSerializer.Serialize(loginResponse);

        await _jsRuntime.InvokeVoidAsync(
            "localStorage.setItem",
            UserStorageKey,
            userJson
        );

        AuthenticationStateChanged?.Invoke();
    }
}
