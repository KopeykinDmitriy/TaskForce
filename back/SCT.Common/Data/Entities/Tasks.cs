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
        // Связи
        public Project Project { get; set; } = null!;

        public ICollection<TaskRelation> TaskRelations1 { get; set; } = new List<TaskRelation>(); // Как Task1
        public ICollection<TaskRelation> TaskRelations2 { get; set; } = new List<TaskRelation>(); // Как Task2
        public ICollection<TaskTags> TaskTags { get; set; } = new List<TaskTags>();
    }
}
