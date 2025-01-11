using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Core.Repositories;
using SCT.TaskManager.Core.Services;
using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksRepository _tasksRepository;
    private readonly DatabaseContext _context;
    
    public TasksController(ITasksRepository tasksRepository, DatabaseContext context)
    {
        _tasksRepository = tasksRepository;
        _context = context;
    }

    [HttpGet("GetTask/{taskId}")]
    public async Task<IActionResult> GetAsync(int taskId)
    {
        var task = await _tasksRepository.GetAsync(taskId);
        if (task == null) 
            return NotFound();
        return Ok(task);
    }

    [HttpPost("AddTask")]
    public async Task AddAsync(TaskDto task)
    {
       await _tasksRepository.AddAsync(task);
    }

    [HttpPost("UpdateTask")]
    public async Task<IActionResult> UpdateAsync(TaskDto updatedTask)
    {
        // Обновляем задачу
        await _tasksRepository.UpdateAsync(updatedTask);

        return Ok(new { message = "Task updated successfully" });
    }
    
    [HttpGet("GetAllTasksByProject")]
    public async Task<IActionResult> GetAllTasksByProjectAsync(int projectId)
    {   
        return Ok(await _tasksRepository.GetAllAsync(projectId));
    }

    [HttpGet("GetAllTags")]
    public async Task<List<string>> GetAllTagsAsync()
    {
        return await _context.Tags.Select(t => t.Name).ToListAsync();
    }

    [HttpGet("Export/{projectId}")]
    public async Task<IActionResult> ExportProjectTasks(int projectId)
    {
        try
        {
            var exporter = new ExportTasksToExcel();
            var excelStream = await exporter.ExportProjectTasksAsync(_tasksRepository, projectId);

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProjectTasks.xlsx");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

