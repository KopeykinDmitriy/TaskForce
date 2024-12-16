using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.Common.Data.Entities;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.DTO;
using SCT.TaskManager.Extensions;

namespace SCT.TaskManager.Core.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private List<ProjectDto> _projects = new();
    private readonly DatabaseContext _context;

    public ProjectsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProjectDto project)
    {
        await InitializeAsync();
        var projectEntity = await _context.Projects.AddAsync(project.MapToEntity());
        await _context.SaveChangesAsync();
        project.Id = projectEntity.Entity.Id;
        _projects.Add(project);
    }
    
    public async Task<ProjectDto?> GetAsync(int id)
    {
        await InitializeAsync();
        return _projects.FirstOrDefault(t => t.Id == id);
    }
    
    public async Task UpdateAsync(ProjectDto updatedProject)
    {
        await InitializeAsync();
        var projectIndex = _projects.FindIndex(t => t.Id == updatedProject.Id);
        if (projectIndex == -1)
            throw new InvalidOperationException("Project not found");
        
        _context.Projects.Update(updatedProject.MapToEntity());
        await _context.SaveChangesAsync();
        _projects[projectIndex] = updatedProject; 
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        await InitializeAsync();
        return _projects;
    }
    
    private async Task InitializeAsync()
    {
        if (_projects.Any())
            return;

        var projects = await _context.Projects.Include(p => p.UserProjects)
                                                         .Include(p => p.Tasks)
                                                         .ToListAsync();
        var projectDtos = projects.Select(p => p.MapToDto()).ToList();
        _projects = projectDtos;
    }
}