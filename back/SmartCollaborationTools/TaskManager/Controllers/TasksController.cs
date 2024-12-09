using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Interfaces.History;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ITaskChangesLogger _taskChangesLogger;
    public TasksController(ITasksRepository tasksRepository, ITaskChangesLogger taskChangesLogger)
    {
        _tasksRepository = tasksRepository;
        _taskChangesLogger = taskChangesLogger;
    }

    [HttpGet("GetTask/{taskId}")]
    public IActionResult Get(int taskId)
    {
        var task = _tasksRepository.Get(taskId);
        if (task == null) 
            return NotFound();
        return Ok(task);
    }

    [HttpPost("AddTask")]
    public void Add(TaskModel task)
    {
        _tasksRepository.Add(task);
    }

    [HttpPost("UpdateTask/{taskId}")]
    public IActionResult Update(int taskId, TaskModel updatedTask)
    {
        var originalTask = _tasksRepository.Get(taskId);

        if (originalTask == null)
            return NotFound();

        // Логируем изменения
        _taskChangesLogger.Log(originalTask, updatedTask);

        // Обновляем задачу
        _tasksRepository.Update(taskId, updatedTask);

        return Ok(new { message = "Task updated successfully" });
    }

    [HttpGet("GetTaskHistory/{taskId}")]
    public IActionResult GetHistory(int taskId)
    {
        var task = _tasksRepository.Get(taskId);

        if (task == null)
            return NotFound();

        return Ok(task.History);
    }
    
    [HttpGet("GetAllTasks")]
    public IActionResult GetAllTasks()
    {
        return Ok(_tasksRepository.GetAll());
    }
}
