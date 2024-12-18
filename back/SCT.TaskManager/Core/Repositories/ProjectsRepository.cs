using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Core.Providers;
using SCT.TaskManager.DTO;
using SCT.TaskManager.Extensions;

namespace SCT.TaskManager.Core.Repositories;

public class ProjectsRepository : IProjectsRepository
{
    private List<ProjectDto> _projects = new();
    private readonly DatabaseContext _context;
    private readonly IUsernameProvider _usernameProvider;

    public ProjectsRepository(DatabaseContext context, IUsernameProvider usernameProvider)
    {
        _context = context;
        _usernameProvider = usernameProvider;
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
        var user = await _context.Users.FirstOrDefaultAsync(u => string.Equals(u.name.ToLower(), _usernameProvider.Get().ToLower()));
        var availableProjects = GetAvailableProjects(user.role == "admin");
        foreach (var availableProject in availableProjects)
        {
            availableProject.UserNames = [];
        }
        return availableProjects;
    }

    private List<ProjectDto> GetAvailableProjects(bool isAdmin)
    {
        if (isAdmin)
            return _projects;
        
        var availableProjects = _projects.Where(p => p.UserNames.Contains(_usernameProvider.Get().ToLower())).ToList();
        return availableProjects;
    }
    
    private async Task InitializeAsync()
    {
        var projects = await _context.Projects.Include(p => p.UserProjects).ThenInclude(up => up.User)
                                              .Include(p => p.Tasks)
                                              .ToListAsync();
        var projectDtos = projects.Select(p => p.MapToDto()).ToList();
        _projects = projectDtos;
    }
}