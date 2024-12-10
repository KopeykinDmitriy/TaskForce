using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Models;

namespace SCT.TaskManager.Core.Repositories;

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