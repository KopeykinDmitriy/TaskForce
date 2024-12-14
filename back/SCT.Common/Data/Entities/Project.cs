namespace SCT.Common.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set;}

        // Связи
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public List<Tasks> Tasks { get; set; } = new List<Tasks>();
        // public ICollection<User> Users { get; set; } = new List<User>();
        // public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>(); 
    }
}