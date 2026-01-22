using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Backend.Models;
using Backend.Enums;
namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TodoItem> TodoItems { get; set; } = null!;


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TodoItems)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
            string commonPasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "ivan_admin", Email = "ivan@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 2, UserName = "maria_dev", Email = "maria@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 3, UserName = "alex_manager", Email = "alex@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 4, UserName = "dmitry_test", Email = "dmitry@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 5, UserName = "elena_design", Email = "elena@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 6, UserName = "sergey_tech", Email = "sergey@example.com", PasswordHash = commonPasswordHash, Role = Role.Admin },
                new User { Id = 7, UserName = "olga_hr", Email = "olga@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 8, UserName = "igor_sales", Email = "igor@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 9, UserName = "anna_support", Email = "anna@example.com", PasswordHash = commonPasswordHash, Role = Role.User },
                new User { Id = 10, UserName = "pavel_backend", Email = "pavel@example.com", PasswordHash = commonPasswordHash, Role = Role.User }
            );

            // --- 10 РЕАЛИСТИЧНЫХ ЗАДАЧ ---
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem { Id = 1, Title = "Обновить серверную ОС", IsCompleted = false, UserId = 1 },
                new TodoItem { Id = 2, Title = "Исправить баг в корзине", IsCompleted = true, UserId = 2 },
                new TodoItem { Id = 3, Title = "Подготовить отчет за квартал", IsCompleted = false, UserId = 3 },
                new TodoItem { Id = 4, Title = "Протестировать API авторизации", IsCompleted = false, UserId = 4 },
                new TodoItem { Id = 5, Title = "Создать макет главной страницы", IsCompleted = true, UserId = 5 },
                new TodoItem { Id = 6, Title = "Настроить CI/CD пайплайны", IsCompleted = false, UserId = 6 },
                new TodoItem { Id = 7, Title = "Провести собеседование с кандидатом", IsCompleted = false, UserId = 7 },
                new TodoItem { Id = 8, Title = "Позвонить ключевому клиенту", IsCompleted = true, UserId = 8 },
                new TodoItem { Id = 9, Title = "Ответить на тикеты в Jira", IsCompleted = false, UserId = 9 },
                new TodoItem { Id = 10, Title = "Оптимизировать SQL запросы", IsCompleted = false, UserId = 10 }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.ConfigureWarnings(w => 
        w.Ignore(RelationalEventId.PendingModelChangesWarning));
}
    }
}