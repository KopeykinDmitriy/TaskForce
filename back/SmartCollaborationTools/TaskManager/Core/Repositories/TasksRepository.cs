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
    
    public TaskModel? Get(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }
    
    public void Update(int id, TaskModel updatedTask)
    {
        var taskIndex = _tasks.FindIndex(t => t.Id == id);
        if (taskIndex == -1)
            throw new InvalidOperationException("Task not found");

        _tasks[taskIndex] = updatedTask; // Замена существующей задачи
    }
}