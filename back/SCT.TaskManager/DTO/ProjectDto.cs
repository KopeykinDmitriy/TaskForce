namespace SCT.TaskManager.DTO;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int UsersCount { get; set; }
    public int TasksCount { get; set; }
    public List<string> UserNames { get; set; }
}