using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Models;

namespace TaskManager.Controllers;

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
    public IActionResult Get(int projectId)
    {
        var project = _projectsRepository.Get(projectId);
        if (project == null) 
            return NotFound();
        return Ok(project);
    }

    [HttpPost("AddProject")]
    public void Add(ProjectModel project)
    {
        _projectsRepository.Add(project);
    }

    [HttpPost("UpdateProject/{projectId}")]
    public IActionResult Update(int projectId, ProjectModel updatedProject)
    {
        var originalProject = _projectsRepository.Get(projectId);

        if (originalProject == null)
            return NotFound();

        // Обновляем задачу
        _projectsRepository.Update(projectId, updatedProject);

        return Ok(new { message = "Project updated successfully" });
    }

    [HttpGet("GetAllProjects")]
    public IActionResult GetAllProjects()
    {
        return Ok(_projectsRepository.GetAll());
    }
}