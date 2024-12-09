namespace TaskManager.Models;

public class ProjectModel
{
    /// <summary>
    /// Уникальный идентификатор проекта
    /// </summary>
    public int Id { get; set; } 
    
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; } = string.Empty; 
    
    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; } = string.Empty; 
}