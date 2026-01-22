using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;

namespace Frontend.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    // Метод для входа
    public async Task<bool> Login(string userName, string password)
    {
        var loginData = new { UserName = userName, Password = password };
        
        // Отправляем POST запрос на твой API (Backend)
        var response = await _http.PostAsJsonAsync("/auth/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                // Сохраняем полученный JWT токен в браузере
                await _localStorage.SetItemAsync("authToken", result.Token);
                
                return true;
            }
        }
        return false;
    }

    // Метод для регистрации (пригодится на следующем шаге)
    public async Task<bool> Register(string email, string password, string username)
    {
        var registerData = new { Email = email, Password = password, Username = username };
        var response = await _http.PostAsJsonAsync("/auth/register", registerData);
        return response.IsSuccessStatusCode;
    }

    // Метод для выхода
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
    }
}

// Вспомогательный класс для чтения токена из ответа API
public class LoginResponse
{ 
    [JsonPropertyName("token")]
    public string Token { get; set; } = ""; 
}