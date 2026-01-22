using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Frontend.Models;

namespace Frontend.Services;

public class AdminService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AdminService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task SetAuthHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Получить все задачи всех пользователей (используем DTO с именами)
    public async Task<List<TodoAdminResponseDto>> GetAllTodosAsync()
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<List<TodoAdminResponseDto>>("todos") ?? new();
    }

    // Поиск задач по имени пользователя (тоже должен возвращать DTO для таблицы)
    public async Task<List<TodoAdminResponseDto>> SearchByUsernameAsync(string username)
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<List<TodoAdminResponseDto>>($"todos/search/{username}") ?? new();
    }

    // Создать задачу для любого пользователя
    public async Task<bool> CreateTodoAsync(string title, int userId)
    {
        await SetAuthHeader();
        var response = await _http.PostAsJsonAsync("todos", new { Title = title, UserId = userId });
        return response.IsSuccessStatusCode;
    }

    // Удалить любую задачу
    public async Task<bool> DeleteTodoAsync(int id)
    {
        await SetAuthHeader();
        var response = await _http.DeleteAsync($"todos/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<List<UserDto>>("api/users") ?? new(); 
    }

    // Обновить задачу
    public async Task<bool> UpdateTodoAsync(int id, string title, int userId)
    {
        await SetAuthHeader();
        var response = await _http.PutAsJsonAsync($"todos/{id}", new { Title = title, UserId = userId });
        return response.IsSuccessStatusCode;
    }
}