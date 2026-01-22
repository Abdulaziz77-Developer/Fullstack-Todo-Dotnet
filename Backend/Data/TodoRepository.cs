using Microsoft.EntityFrameworkCore;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Data
{
    public class TodoRepository
    {
        private AppDbContext _context;
        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoUserResponseDto>> GetTodosUser()
        {
            var todos = await _context.TodoItems
                         .Select(t => new TodoUserResponseDto
                         (
                                t.Id,
                                t.Title,
                                t.IsCompleted
                         )).ToListAsync();
            return todos;
        }
        public async Task<List<TodoAdminResponseDto>> GetTodosAdmin()
        {
            var todos = await _context.TodoItems
                         .Select(t => new TodoAdminResponseDto
                         (
                                t.Id,
                                t.Title,
                                t.User.UserName,
                                t.User.Email,
                                t.IsCompleted
                         )).ToListAsync();
            return todos;
        }
        public async Task<List<TodoUserResponseDto>> GetTodoByUserId(int userid)
        {
            var todo = await _context.TodoItems
                         .Where(t => t.UserId == userid)
                         .Select(t => new TodoUserResponseDto
                         (
                                t.Id,
                                t.Title,
                                t.IsCompleted
                         )).ToListAsync();

            return todo;
        }
        public async Task<bool> AddAsync(TodoItem todo)
        {
            _context.TodoItems.Add(todo);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }
        public async Task<bool> UpdateAsync(int id, UpdateTodoDto todo)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) return false;
            todoItem.Title = todo.Title;
            todoItem.UserId = todo.UserId;
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) return false;
            _context.TodoItems.Remove(todoItem);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }
        public async Task<List<TodoAdminResponseDto>> GetTodosByNameAsync(string title)
        {
            return await _context.TodoItems
                         .Where(t => t.Title.Contains(title))
                         .Select(t => new TodoAdminResponseDto
                         (
                                t.Id,
                                t.Title,
                                t.User.UserName,
                                t.User.Email,
                                t.IsCompleted
                         )).ToListAsync();
        }
        public async Task<TodoItem?> GetTodoByIdAsync(int id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<List<TodoAdminResponseDto>> GetByUserNameAsync(string userName)
        {
            return await _context.TodoItems
                         .Where(t => t.User.UserName == userName)
                         .Select(t => new TodoAdminResponseDto
                         (
                                t.Id,
                                t.Title,
                                t.User.UserName,
                                t.User.Email,
                                t.IsCompleted
                         )).ToListAsync();
        }   
    }
}