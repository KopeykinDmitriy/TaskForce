namespace DatabaseService.Data.Entities
{
    public class TaskTags
    {
        //public required int Id { get; set; }
        public required int TaskId { get; set; }
        public required int TagId { get; set; }

        public required Tasks Task { get; set; }
        public required Tag Tag { get; set; }
    }
}
