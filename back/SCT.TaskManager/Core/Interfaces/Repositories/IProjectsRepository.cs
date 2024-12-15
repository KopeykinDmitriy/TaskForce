using SCT.Common.Data.Entities;
using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Core.Interfaces.Repositories;
public interface IProjectsRepository
{
    Task AddAsync(ProjectDto task);
    Task<ProjectDto?> GetAsync(int id);
    Task UpdateAsync(ProjectDto updatedTask);
    Task<List<ProjectDto>> GetAllAsync();
}