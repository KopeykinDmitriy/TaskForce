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

    [HttpGet(Name = "GetTask")]
    public TaskModel Get(int taskId)
    {
        return _tasksRepository.Get(taskId);
    }
   
    [HttpPost(Name = "AddTask")]
    public void Add(TaskModel task)
    {
        _tasksRepository.Add(task);
    }
}