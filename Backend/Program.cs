
// using MinimalApiTodo.Data;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using MinimalApiTodo.EndPoints;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//         };
//     });
// builder.Services.AddScoped<TodoRepository>();
// builder.Services.AddAuthorization();
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
// });
// var app = builder.Build();
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.Use(async (context, next) =>
// {
//     Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
//     await next();
// });
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapTodoEndPoints();
// app.Run();
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Для OpenApiInfo и Security моделей
using Backend.Data;
using Backend.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// --- 1. СЕРВИСЫ И SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Настройка информации об API
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });

    // Описываем схему безопасности (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите токен. Пример: Bearer 12345abcdef"
    });

    // Делаем замок активным для всех эндпоинтов
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- 2. АУТЕНТИФИКАЦИЯ ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// --- 3. ОСТАЛЬНЫЕ СЕРВИСЫ ---
builder.Services.AddScoped<TodoRepository>();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// --- 4. MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"[LOG]: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// РЕГИСТРАЦИЯ ЭНДПОИНТОВ
app.MapAuthEndpoints(builder.Configuration); // Эндпоинты логина и регистрации
app.MapTodoEndPoints();

app.Run();