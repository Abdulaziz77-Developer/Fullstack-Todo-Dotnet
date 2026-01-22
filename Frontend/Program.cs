using Frontend.Components;
using Frontend.Services;
using Blazored.LocalStorage;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient 
{ 
    // Замени 5123 на тот порт, который ты нашел в бэкенде
    BaseAddress = new Uri("http://localhost:5119/") 
});
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TodoUserService>();
builder.Services.AddScoped<AdminService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
