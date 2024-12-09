using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksRepository _tasksRepository;

    public TasksController(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }

    [HttpGet("{taskId}", Name = "GetTask")]
    public TaskModel Get(int taskId)
    {
        return _tasksRepository.Get(taskId);
    }

    [HttpPost(Name = "AddTask")]
    public void Add(TaskModel task)
    {
        _tasksRepository.Add(task);
    }

    [HttpPut("{taskId}", Name = "UpdateTask")]
    public IActionResult Update(int taskId, TaskModel updatedTask)
    {
        try
        {
            var originalTask = _tasksRepository.Get(taskId);

            if (originalTask == null)
                return NotFound();

            // Логируем изменения
            LogChanges(originalTask, updatedTask);

            // Обновляем задачу
            _tasksRepository.Update(taskId, updatedTask);

            return Ok(new { message = "Task updated successfully" });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { message = "Task not found" });
        }
    }

    [HttpGet("{taskId}/history", Name = "GetTaskHistory")]
    public IActionResult GetHistory(int taskId)
    {
        var task = _tasksRepository.Get(taskId);

        if (task == null)
            return NotFound();

        return Ok(task.History);
    }

    private void LogChanges(TaskModel originalTask, TaskModel updatedTask)
    {
        if (originalTask.Name != updatedTask.Name)
        {
            originalTask.History.Add(new ChangeLog
            {
                ChangedBy = "CurrentUser", // Реального пользователя можно добавить через контекст
                FieldChanged = "Name",
                OldValue = originalTask.Name,
                NewValue = updatedTask.Name,
                
            });
        }

        if (originalTask.Description != updatedTask.Description)
        {
            originalTask.History.Add(new ChangeLog
            {
                ChangedBy = "CurrentUser",
                FieldChanged = "Description",
                OldValue = originalTask.Description,
                NewValue = updatedTask.Description,
                
            });
        }

        if (originalTask.Status != updatedTask.Status)
        {
            originalTask.History.Add(new ChangeLog
            {
                ChangedBy = "CurrentUser",
                FieldChanged = "Status",
                OldValue = originalTask.Status,
                NewValue = updatedTask.Status,
                
            });
        }

        if (originalTask.Priority != updatedTask.Priority)
        {
            originalTask.History.Add(new ChangeLog
            {
                ChangedBy = "CurrentUser",
                FieldChanged = "Priority",
                OldValue = originalTask.Priority,
                NewValue = updatedTask.Priority,
                
            });
        }

        // Аналогично добавьте проверки для других полей, если требуется
    }
}
