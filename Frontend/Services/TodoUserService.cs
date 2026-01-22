using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Frontend.Models;

namespace Frontend.Services;

public class TodoUserService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public TodoUserService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    // Вспомогательный метод для добавления токена в запрос
    private async Task AddAuthHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<TodoItem>> GetMyTodosAsync()
    {
        await AddAuthHeader();
        return await _http.GetFromJsonAsync<List<TodoItem>>("todos/user") ?? new();
    }

    public async Task<bool> ToggleTodoAsync(int id)
    {
        await AddAuthHeader();
        var response = await _http.PutAsync($"todos/{id}/toggle", null);
        return response.IsSuccessStatusCode;
    }
}