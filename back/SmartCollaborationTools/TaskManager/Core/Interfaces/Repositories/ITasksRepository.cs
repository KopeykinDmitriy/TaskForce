using TaskManager.Models;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ITasksRepository
{
    void Add(TaskModel task);
    TaskModel? Get(int id);
    void Update(int id, TaskModel updatedTask);
    List<TaskModel> GetAll();
}