using System.Text.Json.Serialization;
using SCT.TaskManager.Core.Enums;

namespace SCT.TaskManager.DTO;

public class TaskDto

{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDateTime { get; set; } 
    public DateTimeOffset EndDateTime { get; set; }
    public string Status { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskPriority Priority { get; set; }
    public string CreatorName { get; set; }
    public string ExecutorName { get; set; }
    public int ProjectId { get; set; }
    public List<string> Tags { get; set; }
}