using TaskManager.Core.History;
using TaskManager.Core.Interfaces.History;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Repositories;

namespace TaskManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddSingleton<ITasksRepository, TasksRepository>();
        builder.Services.AddSingleton<ITaskChangesLogger, TaskChangesLogger>();
        builder.Services.AddSingleton<IProjectsRepository, ProjectsRepository>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
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