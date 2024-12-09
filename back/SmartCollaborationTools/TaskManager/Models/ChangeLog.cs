namespace TaskManager.Models;

public class ChangeLog
{
    public int Id { get; set; } // Уникальный идентификатор записи
    public DateTime ChangeDate { get; set; } = DateTime.UtcNow; // Дата изменения
    public string ChangedBy { get; set; } = string.Empty; // Автор изменения
    public string FieldChanged { get; set; } = string.Empty; // Изменённое поле
    public string OldValue { get; set; } = string.Empty; // Значение до изменения
    public string NewValue { get; set; } = string.Empty; // Значение после изменения
}