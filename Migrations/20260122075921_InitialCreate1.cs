using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MinimalApiTodo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "ivan@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "ivan_admin" },
                    { 2, "maria@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "maria_dev" },
                    { 3, "alex@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "alex_manager" },
                    { 4, "dmitry@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "dmitry_test" },
                    { 5, "elena@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "elena_design" },
                    { 6, "sergey@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 0, "sergey_tech" },
                    { 7, "olga@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "olga_hr" },
                    { 8, "igor@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "igor_sales" },
                    { 9, "anna@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "anna_support" },
                    { 10, "pavel@example.com", "$2a$11$B6ixhifeLH9vSKmF72RHduPKA9whE.hLzCO.qksGFmmPCLPjV4rDq", 1, "pavel_backend" }
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "IsCompleted", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, false, "Обновить серверную ОС", 1 },
                    { 2, true, "Исправить баг в корзине", 2 },
                    { 3, false, "Подготовить отчет за квартал", 3 },
                    { 4, false, "Протестировать API авторизации", 4 },
                    { 5, true, "Создать макет главной страницы", 5 },
                    { 6, false, "Настроить CI/CD пайплайны", 6 },
                    { 7, false, "Провести собеседование с кандидатом", 7 },
                    { 8, true, "Позвонить ключевому клиенту", 8 },
                    { 9, false, "Ответить на тикеты в Jira", 9 },
                    { 10, false, "Оптимизировать SQL запросы", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
