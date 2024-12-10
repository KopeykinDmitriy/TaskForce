using SCT.TaskManager.Models;

namespace SCT.TaskManager.Core.Interfaces.Repositories;

public interface ITasksRepository
{
    void Add(TaskModel task);
    TaskModel Get(int id);
}