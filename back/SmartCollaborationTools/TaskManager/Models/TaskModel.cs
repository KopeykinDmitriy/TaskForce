namespace TaskManager.Models;

public class TaskModel
{
    public int Id { get; set; } // Уникальный идентификатор задачи
    public string Name { get; set; } = string.Empty; // Название задачи
    public string Description { get; set; } = string.Empty; // Описание задачи
    public string Status { get; set; } = "TO_DO"; // Статус задачи (TO_DO, IN_PROGRESS, COMPLETED)
    public string Creator { get; set; } = string.Empty; // Создатель задачи
    public string Assignee { get; set; } = string.Empty; // Исполнитель задачи
    public DateTime DueDate { get; set; } // Срок выполнения задачи
    public string Project { get; set; } = string.Empty; // Проект, к которому относится задача
    public string Priority { get; set; } = "Normal"; // Приоритет задачи (например, High, Normal, Low)
    public List<string> Tags { get; set; } = new(); // Теги задачи
    public List<ChangeLog> History { get; set; } = new(); // История изменений задачи

    public void UpdateField(string field, string oldValue, string newValue, string changedBy)
    {
        if (oldValue != newValue)
        {
            History.Add(new ChangeLog
            {
                FieldChanged = field,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedBy = changedBy,
                ChangeDate = DateTime.UtcNow
            });
        }
    }
}