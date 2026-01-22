using System.Text.Json.Serialization;

namespace Frontend.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
}