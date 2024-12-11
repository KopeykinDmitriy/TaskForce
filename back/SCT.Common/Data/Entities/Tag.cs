namespace SCT.Common.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TaskTags> TaskTags { get; set; } = new List<TaskTags>();
        public ICollection<UserTags> UserTags { get; set; } = new List<UserTags>();

    }
}
