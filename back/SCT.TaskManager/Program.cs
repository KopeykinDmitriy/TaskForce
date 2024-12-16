using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCT.Common.Data.DatabaseContext;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Core.Providers;
using SCT.TaskManager.Core.Repositories;

namespace SCT.TaskManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                              builder =>
                              {
                                  builder
                                      .WithOrigins("http://localhost:3000") // Разрешаем все домены
                                      .AllowAnyHeader() // Разрешаем любые заголовки
                                      .AllowAnyMethod() // Разрешаем любые методы
                                      .AllowCredentials(); // Разрешаем куки и авторизацию
                              });
        });

        // Add services to the container.
        builder.Services.AddSingleton<ITasksRepository, TasksRepository>();
        builder.Services.AddSingleton<IProjectsRepository, ProjectsRepository>();
        builder.Services.AddSingleton<IUsernameProvider, UsernameProvider>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("SCT.TaskManager")
            ),
            ServiceLifetime.Singleton,
            ServiceLifetime.Singleton);
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                //Description = "Введите JWT токен следующим образом: Bearer {токен}",
                //Name = "Authorization",
                //In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                //Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                //Scheme = "bearer"
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Please enter the JWT with Bearer keyword"
            });
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
        
        // Настройка аутентификации и авторизации
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Authentication:Authority"];
                options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Authentication:RequireHttpsMetadata"] ?? "false");
                options.Audience = builder.Configuration["Authentication:Aud"];
                options.SaveToken = bool.Parse(builder.Configuration["Authentication:SaveToken"] ?? "true");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false 
                };
            });
        
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        var app = builder.Build();
        // Выполнение миграций при старте
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();
        }
        
        // Configure the HTTP request pipeline.
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" } };
            });
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "My API V1");
        });
        
        app.UseCors("AllowSpecificOrigin");
        

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}