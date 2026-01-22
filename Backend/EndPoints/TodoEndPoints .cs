using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Enums;
namespace Backend.EndPoints
{
    public static class TodoEndPoints
    {
        public static void MapTodoEndPoints(this WebApplication app)
        {
            app.MapGet("/users", async (AppDbContext db) => 
                await db.Users
                .Select(u => new UserDto { Id = u.Id, UserName = u.UserName })
                .ToListAsync())
                .RequireAuthorization();
            var group = app.MapGroup("/todos").RequireAuthorization();

            group.MapGet("/todos/{username}", async (string username, TodoRepository repository) =>
            {
                var todos = await repository.GetTodosByNameAsync(username);
                return Results.Ok(todos);
            });
            // group.MapGet("/users", async (ClaimsPrincipal user, TodoRepository repository) =>
            // {
            //     var users = repository.GetTodosAdmin();
            //     return Results.Ok(users);
            // });
            group.MapGet("/user", async (ClaimsPrincipal user, TodoRepository repository) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var todos = await repository.GetTodoByUserId(int.Parse(userId));
                return Results.Ok(todos);
            }).RequireAuthorization();
            
            group.MapGet("/", async (ClaimsPrincipal user, TodoRepository repository) =>
            {
                if (user.IsInRole("Admin"))
                {
                    var todos = await repository.GetTodosAdmin();
                    return Results.Ok(todos);
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

            app.MapPut("/todos/{id}/toggle", async (int id, ClaimsPrincipal user, AppDbContext db) =>
            {
                // 1. Извлекаем ID пользователя из токена
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return Results.Unauthorized();
                int userId = int.Parse(userIdClaim);

                // 2. Ищем задачу, которая принадлежит ИМЕННО этому пользователю
                var todo = await db.TodoItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                // 3. Если задачи нет (или она чужая)
                if (todo is null) return Results.NotFound("Задача не найдена.");

                // 4. Меняем статус
                todo.IsCompleted = !todo.IsCompleted;

                // 5. Сохраняем в БД
                await db.SaveChangesAsync();

                return Results.Ok(todo);
            }).RequireAuthorization();

            group.MapPost("/", async (CreateTodoDto todoDto, TodoRepository repository, ClaimsPrincipal user) =>
            {
                if (user.IsInRole("Admin"))
                {
                    var todo = new TodoItem
                    {
                        Title = todoDto.Title,
                        IsCompleted = false,
                        UserId = todoDto.UserId
                    };
                    var created = await repository.AddAsync(todo);
                    if (created)
                    {
                        return Results.Created($"/todos/{todo.Id}", todo);
                    }
                    return Results.BadRequest("Could not create the todo item.");
                }
                else
                {
                    return Results.Unauthorized();
                }

            });

            group.MapPut("/{id}", async (int id, UpdateTodoDto todoDto, TodoRepository repository, ClaimsPrincipal user) =>
            {
                if (user.IsInRole("Admin"))
                {
                    var updated = await repository.UpdateAsync(id, todoDto);
                    if (updated)
                    {
                        return Results.NoContent();
                    }
                    return Results.NotFound("Todo item not found.");
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

            group.MapDelete("/{id}", async (int id, TodoRepository repository, ClaimsPrincipal user) =>
            {
                if (!user.IsInRole("Admin"))
                {
                    return Results.Unauthorized();
                }
                var deleted = await repository.DeleteTodoAsync(id);
                if (deleted)
                {
                    return Results.NoContent();
                }
                return Results.NotFound("Todo item not found.");
            });

            group.MapGet("/{searchUserName}", async (string searchUserName, TodoRepository repository, ClaimsPrincipal user) =>
            {
                if (!user.IsInRole("Admin"))
                {
                    return Results.Unauthorized();
                }
                var todos = await repository.GetTodosByNameAsync(searchUserName);
                return Results.Ok(todos);
            });
            
            group.MapGet("/search/{searchWithTodoTitle}", async (string searchWithTodoTitle, TodoRepository repository, ClaimsPrincipal user) =>
            {
                if (!user.IsInRole("Admin"))
                {
                    return Results.Unauthorized();
                }
                var todos = await repository.GetTodosByNameAsync(searchWithTodoTitle);
                return Results.Ok(todos);
            });
        }
    }
}
