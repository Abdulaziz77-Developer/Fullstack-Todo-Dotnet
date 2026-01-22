namespace MinimalApiTodo.DTOs
{
    public record UserLoginDto(string UserName, string Password);

    // То, что мы отдаем в API при запросе данных о пользователе (без пароля!)
    public record UserResponseDto(
        int Id,
        string UserName,
        string Role
    );
    public record TodoAdminResponseDto(
        int Id,
        string Title,
        string AssignedToUserName, // Имя исполнителя
        string AssignedToEmail,
        bool IsCompleted    
    );
    public record CreateTodoDto(
        string Title,
        int UserId // ID пользователя, которому назначаем задачу
    );
    public record UpdateTodoDto(
        string Title,
        int UserId
    );
    public record TodoUserResponseDto(
        int Id,
        string Title,
        bool IsCompleted
    );
}