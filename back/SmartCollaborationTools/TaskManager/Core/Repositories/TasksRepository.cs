using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Models;

namespace TaskManager.Core.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly List<TaskModel> _tasks = new();

    public void Add(TaskModel task)
    {
        _tasks.Add(task);
    }

    public TaskModel Get(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id) ?? throw new InvalidOperationException();
    }
}