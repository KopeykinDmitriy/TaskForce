using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Core.Interfaces.Repositories;

public interface ITasksRepository
{
    Task AddAsync(TaskDto task);
    Task<TaskDto?> GetAsync(int id);
    Task UpdateAsync(TaskDto updatedTask);
    Task<List<TaskDto>> GetAllAsync(int projectId);
}