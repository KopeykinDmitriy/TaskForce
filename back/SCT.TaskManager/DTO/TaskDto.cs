namespace SCT.TaskManager.DTO;

public class TaskDto

{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset StartDateTime { get; set; } 
    public DateTimeOffset EndDateTime { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
    public string CreatorName { get; set; }
    public string ExecutorName { get; set; }
    public int ProjectId { get; set; }
    public List<string> Tags { get; set; }
}