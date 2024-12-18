namespace SCT.Common.Data.Entities
{
    public class User
    {
        public int id { get; set; }
        public required string name { get; set; }
        public string? surname {get; set;}
        public string? email { get; set;}
        public string? role { get; set; }


        // связи с пользователями
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public ICollection<UserTags> UserTags { get; set; } = new List<UserTags>();
        public ICollection<Tasks> UserCreate { get; set; } = new List<Tasks>();
        public ICollection<Tasks> UserDo { get; set; } = new List<Tasks>();
    }
}