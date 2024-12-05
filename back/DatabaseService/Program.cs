using Microsoft.EntityFrameworkCore;
using DatabaseService.Data.DatabaseContext;
using DatabaseService.Repositories;
using DatabaseService.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер зависимостей
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Настройка подключения к базе данных PostgreSQL

builder.Services.AddScoped<UserRepository>(); // Регистрация репозитория
builder.Services.AddScoped<UserService>();    // Регистрация сервиса

builder.Services.AddControllers(); // Регистрация контроллеров (API)

builder.Services.AddEndpointsApiExplorer(); // Для поддержки Swagger
builder.Services.AddSwaggerGen(); // Для Swagger документации

var app = builder.Build();

// Настройка Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Настройка редиректа на HTTPS

app.MapControllers(); // Маршруты контроллеров

app.Run();
