namespace SCT.Common.Data.Entities
{
    public class TaskRelation
    {
        public int Id { get; set; }
        public required int IdTask1 { get; set; }
        public required int IdTask2 { get; set; }
        public required string Relation { get; set; }


        public required Tasks Task1 { get; set; } = null!; // Первая задача
        public required Tasks Task2 { get; set; } = null!; // Вторая задача


    }
}
