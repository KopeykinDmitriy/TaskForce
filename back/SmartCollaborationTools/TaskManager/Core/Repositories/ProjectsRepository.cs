using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Models;

namespace TaskManager.Core.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private readonly List<ProjectModel> _projects = new();

    public void Add(ProjectModel project)
    {
        _projects.Add(project);
    }
    
    public ProjectModel? Get(int id)
    {
        return _projects.FirstOrDefault(t => t.Id == id);
    }
    
    public void Update(int id, ProjectModel updatedProject)
    {
        var projectIndex = _projects.FindIndex(t => t.Id == id);
        if (projectIndex == -1)
            throw new InvalidOperationException("Project not found");

        _projects[projectIndex] = updatedProject; 
    }

    public List<ProjectModel> GetAll()
    {
        return _projects;
    }
}