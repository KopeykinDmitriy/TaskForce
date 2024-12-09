using TaskManager.Models;

namespace TaskManager.Core.Interfaces.Repositories;
public interface IProjectsRepository
{
    void Add(ProjectModel task);
    ProjectModel? Get(int id);
    void Update(int id, ProjectModel updatedTask);
    List<ProjectModel> GetAll();
}