namespace DatabaseService.Data.Entities
{
    public class User
    {
        public int id { get; set; }
        public required string name { get; set; }
        public string? surname {get; set;}
        public string? email { get; set;}
        public required string password {get; set;}


        // связь с пользователями
        public ICollection<UserProject> UserProjects { get; set; }
    }
}