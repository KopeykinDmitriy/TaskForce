using Microsoft.AspNetCore.Mvc;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : Controller
{
    private readonly IProjectsRepository _projectsRepository;

    public ProjectsController(IProjectsRepository projectsRepository)
    {
        _projectsRepository = projectsRepository;
    }
    
    [HttpGet("GetProject/{projectId}")]
    public async Task<IActionResult> GetAsync(int projectId)
    {
        var project = await _projectsRepository.GetAsync(projectId);
        if (project == null) 
            return NotFound();
        return Ok(project);
    }

    [HttpPost("AddProject")]
    public async Task AddAsync(ProjectDto project)
    {
        await _projectsRepository.AddAsync(project);
    }

    [HttpPost("UpdateProject")]
    public async Task<IActionResult> UpdateAsync(ProjectDto updatedProject)
    {
        // Обновляем задачу
        await _projectsRepository.UpdateAsync(updatedProject);

        return Ok(new { message = "Project updated successfully" });
    }

    [HttpGet("GetAllProjects")]
    public async Task<IActionResult> GetAllProjectsAsync()
    {
        return Ok(await _projectsRepository.GetAllAsync());
    }
}