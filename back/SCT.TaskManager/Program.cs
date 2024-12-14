using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Core.Repositories;

namespace SCT.TaskManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<ITasksRepository, TasksRepository>();
        builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("SCT.TaskManager")
            ));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        // Выполнение миграций при старте
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();
        }
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}