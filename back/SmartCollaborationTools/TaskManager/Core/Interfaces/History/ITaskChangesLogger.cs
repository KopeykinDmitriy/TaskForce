using TaskManager.Models;

namespace TaskManager.Core.Interfaces.History;

public interface ITaskChangesLogger
{
    public void Log(TaskModel originalTask, TaskModel updatedTask);
}