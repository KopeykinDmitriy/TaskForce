namespace SCT.Common.Data.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public required  string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Start_dt {  get; set; }
        public DateTimeOffset End_dt { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }


        // Внешний ключ для связи с проектом
        public int ProjectId { get; set; }
        public required int UserCreateId { get; set; }
        public int? UserDoId { get; set; }


        // Связи
        public Project Project { get; set; } = null!;
        public required User UserCreate { get; set; }
        public User? UserDo { get; set; }
        public ICollection<TaskRelation> TaskRelations1 { get; set; } = new List<TaskRelation>(); // Как Task1
        public ICollection<TaskRelation> TaskRelations2 { get; set; } = new List<TaskRelation>(); // Как Task2
        public ICollection<TaskTags> TaskTags { get; set; } = new List<TaskTags>();
        
    }
}
