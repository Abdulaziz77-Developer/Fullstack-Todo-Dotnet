using MinimalApiTodo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

List<TodoItem>? todoItems = new List<TodoItem>
{
    new TodoItem()
    {
        Id = 1,
        Title = "Did the home work",
        IsCompleted = true,
    },
    new TodoItem()
    {
        Id = 2,
        Title = "Test To do 2",
        IsCompleted = false
    },
    new TodoItem()
    {
        Id = 3,
        Title = "Todo 3 ",
        IsCompleted = false
    }
};
app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
    await next();
});
app.MapGet("/todos/{id}", async (int id) => {
 var todo = todoItems.FirstOrDefault(t => t.Id == id);
return todo is null ? Results.NotFound() : Results.Ok(todo);

});
app.MapGet("/todos", async () => {
    var todos = todoItems.ToList();
    return Results.Ok(todos);
});
app.MapPost("/todos", (TodoItem todo) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(todo.Title) || todo.Title.Length < 3)
        {
            return Results.BadRequest("Title must be at least 3 characters");
        }

        todo.Id = todoItems.Max(t => t.Id) + 1;
        todoItems.Add(todo);

        return Results.Created($"/todos/{todo.Id}", todo);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.Problem("Unexpected server error");
    }
});
app.MapDelete("/todos/{id}", (int id) =>
{
        var todo = todoItems.FirstOrDefault(t => t.Id == id);
        if (todo is null)
        {
            return Results.NotFound();
        }
        todoItems.Remove((TodoItem)todo);
        return Results.Ok(true);
});
app.MapPut("/todos/{id}",  (int id, TodoItem todo) =>
{
    var existingTodo = todoItems.FirstOrDefault(t => t.Id == id);

    if (existingTodo is null)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(todo.Title) || todo.Title.Length < 3)
        return Results.BadRequest("Title must be at least 3 characters");

    existingTodo.Title = todo.Title;
    existingTodo.IsCompleted = todo.IsCompleted;

    return Results.Ok(existingTodo);

});
app.Run();
