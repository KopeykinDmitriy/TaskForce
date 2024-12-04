namespace DatabaseService.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set;}

        // Связь многие ко многим
        public ICollection<UserProject> UserProjects { get; set; }
        // public ICollection<User> Users { get; set; } = new List<User>();
        // Связь "многие ко многим" через таблицу UserProject подумать как лучше реализовать
    // public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>(); 
    }
}