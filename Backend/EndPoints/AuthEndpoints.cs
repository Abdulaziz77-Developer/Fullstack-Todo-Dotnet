using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Enums;
using BCrypt.Net;

namespace Backend.EndPoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app, IConfiguration config)
    {
        var auth = app.MapGroup("/auth");

        // --- РЕГИСТРАЦИЯ ---
        auth.MapPost("/register", async (UserLoginDto dto, AppDbContext db) =>
        {
            if (await db.Users.AnyAsync(u => u.UserName == dto.UserName))
                return Results.BadRequest("User already exists");

            var user = new User
            {
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Role.User // По умолчанию обычный юзер
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Ok("User registered successfully");
        });

        // --- ВХОД (ВЫДАЧА ТОКЕНА) ---
        auth.MapPost("/login", async (UserLoginDto dto, AppDbContext db) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Results.Unauthorized();

            // Создаем Claims (паспортные данные)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // Важно для твоего IsInRole("Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Results.Ok(new { token = jwt });
        });
    }
}