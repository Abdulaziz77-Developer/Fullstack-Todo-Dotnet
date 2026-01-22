using System.Text.Json.Serialization;

namespace Frontend.Models;

// Модель должна точно соответствовать порядку и именам полей из твоего Select на бэкенде
public class TodoAdminResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("assignedToUserName")]
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    // Конструктор для удобства (если понадобится)
    public TodoAdminResponseDto() { }

    public TodoAdminResponseDto(int id, string title, string userName, string email, bool isCompleted)
    {
        Id = id;
        Title = title;
        UserName = userName;
        Email = email;
        IsCompleted = isCompleted;
    }
}