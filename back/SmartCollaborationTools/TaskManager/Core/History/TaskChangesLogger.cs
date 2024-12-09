using TaskManager.Core.Interfaces.History;
using TaskManager.Models;

namespace TaskManager.Core.History;

public class TaskChangesLogger : ITaskChangesLogger
{
    public void Log(TaskModel originalTask, TaskModel updatedTask)
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
    }
}