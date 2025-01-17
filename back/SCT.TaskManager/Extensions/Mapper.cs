using SCT.Common.Data.Entities;
using SCT.TaskManager.Core.Enums;
using SCT.TaskManager.DTO;

namespace SCT.TaskManager.Extensions;

public static class Mapper
{
    public static TaskDto MapToDto(this Tasks task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description ?? string.Empty,
            Status = task.Status ?? "TO_DO",
            StartDateTime = task.Start_dt,
            EndDateTime = task.End_dt,
            Priority = (TaskPriority)task.Priority,
            ProjectId = task.ProjectId,
            Tags = task.TaskTags.Select(tt => tt.Tag.Name).ToList(),
            CreatorName = task.UserCreate.name,
            ExecutorName = task.UserDo?.name ?? string.Empty
        };
    }

    public static Tasks MapToEntity(this TaskDto dto, int userCreatorId, int? userExecutorId)
    {
        return new Tasks
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            Start_dt = DateTimeOffset.Now,
            End_dt = dto.EndDateTime,
            Priority = (int)dto.Priority,
            ProjectId = dto.ProjectId,
            UserCreateId = userCreatorId,
            UserDoId = userExecutorId
        };
    }

    public static ProjectDto MapToDto(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            UsersCount = project.UserProjects.Count(userProject => userProject.ProjectId == project.Id),
            TasksCount = project.Tasks.Count(task => task.ProjectId == project.Id),
            UserNames = project.UserProjects.Select(up => up.User.name.ToLower()).ToList()
        };
    }

    public static Project MapToEntity(this ProjectDto dto)
    {
        return new Project
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
        };
    }
}