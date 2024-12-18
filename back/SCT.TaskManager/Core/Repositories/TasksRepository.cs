using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.Common.Data.Entities;
using SCT.TaskManager.Core.Interfaces.Repositories;
using SCT.TaskManager.Core.Providers;
using SCT.TaskManager.DTO;
using SCT.TaskManager.Extensions;

namespace SCT.TaskManager.Core.Repositories;

public class TasksRepository : ITasksRepository
{
    private List<TaskDto> _tasks = new();
    private readonly DatabaseContext _context;
    private readonly IUsernameProvider _usernameProvider;

    public TasksRepository(DatabaseContext context, IUsernameProvider usernameProvider)
    {
        _context = context;
        _usernameProvider = usernameProvider;
    }

    public async Task AddAsync(TaskDto task)
    {
        await InitializeAsync();
        var userName = _usernameProvider.Get();
        var userCreator = await _context.Users.FirstOrDefaultAsync(u => u.name == userName);
        var userExecutor = string.Equals(task.ExecutorName, string.Empty) 
                               ? null 
                               : await _context.Users.FirstOrDefaultAsync(u => u.name == task.ExecutorName);
        var taskEntity = task.MapToEntity(userCreator.id, userExecutor?.id);
        taskEntity.TaskTags = await GetTaskTagsAsync(task);
        taskEntity.UserCreateId = userCreator.id;
        var newTask = await _context.Tasks.AddAsync(taskEntity);
        await _context.SaveChangesAsync();
        task.Id = newTask.Entity.Id;
        task.CreatorName = newTask.Entity.UserCreate.name;
        task.ExecutorName = newTask.Entity.UserDo?.name ?? string.Empty;
        _tasks.Add(task);
    }
    
    public async Task<TaskDto?> GetAsync(int id)
    {
        await InitializeAsync();
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        var availableTasks = await GetAvailableTasksAsync([task]);
        return availableTasks.FirstOrDefault();
    }
    
    public async Task UpdateAsync(TaskDto updatedTask)
    {
        await InitializeAsync();

        var existingTask = await _context.Tasks
            .Include(t => t.TaskTags)
            .FirstOrDefaultAsync(t => t.Id == updatedTask.Id);

        if (existingTask == null)
            throw new InvalidOperationException("Task not found");
        
        var updatedTaskEntity = updatedTask.MapToEntity(existingTask.UserCreateId, existingTask.UserDoId);

        existingTask.Name = updatedTaskEntity.Name;
        existingTask.Description = updatedTaskEntity.Description;
        existingTask.Start_dt = updatedTaskEntity.Start_dt;
        existingTask.End_dt = updatedTaskEntity.End_dt;
        existingTask.Status = updatedTaskEntity.Status;
        existingTask.Priority = updatedTaskEntity.Priority;

        var newTaskTags = await GetTaskTagsAsync(updatedTask);
        existingTask.TaskTags = newTaskTags;

        await _context.SaveChangesAsync();

        var taskIndex = _tasks.FindIndex(t => t.Id == updatedTask.Id);
        _tasks[taskIndex] = updatedTask;
    }
    
    public async Task<List<TaskDto>> GetAllAsync(int projectId)
    {
        await InitializeAsync();
        var tasks = _tasks.Where(t => t.ProjectId == projectId).ToList();
        var availableTasks = await GetAvailableTasksAsync(tasks);
        return availableTasks;
    }

    private async Task InitializeAsync()
    {
        if (_tasks.Any())
            return;

        var tasks = await _context.Tasks.Include(t => t.TaskTags).ThenInclude(tt => tt.Tag).Include(t => t.UserCreate).Include(t => t.UserDo).ToListAsync();
        var taskDtos = tasks.Select(t => t.MapToDto()).ToList();
        _tasks = taskDtos;
    }

    private async Task<List<TaskTags>> GetTaskTagsAsync(TaskDto task)
    {
        var taskId = task.Id;
        var tagNames = task.Tags;
        var tags = await GetTagsAsync(tagNames);
        return tags.Select(t => new TaskTags { TaskId = taskId, TagId = t }).ToList();
    }

    private async Task<List<int>> GetTagsAsync(List<string> tagNames)
    {
        var tags = await _context.Tags.ToListAsync();
        var oldTags = tags.Where(t => tagNames.Contains(t.Name.ToLower())).ToList();
        var newTagNames = tagNames.Where(t => !oldTags.Select(ot => ot.Name).Contains(t.ToLower())).ToList();
        var newTags = newTagNames.Select(t => new Tag { Name = t.ToLower() }).ToList();
        await _context.Tags.AddRangeAsync(newTags);
        await _context.SaveChangesAsync();
        tags = await _context.Tags.Where(t => tagNames.Contains(t.Name.ToLower())).ToListAsync();
        return tags.Select(t => t.Id).ToList();
    }

    private async Task<List<TaskDto>> GetAvailableTasksAsync(List<TaskDto> tasks)
    {
        var user = await _context.Users.Include(u => u.UserTags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync(u => string.Equals(u.name.ToLower(), _usernameProvider.Get().ToLower()));
        if (user.role == "admin")
            return tasks;
        
        var availableTasks = tasks.Where(t => t.Tags.Any(tag => user.UserTags.Select(ut => ut.Tag.Name.ToLower()).Contains(tag.ToLower())));
        return availableTasks.ToList();
    }
}