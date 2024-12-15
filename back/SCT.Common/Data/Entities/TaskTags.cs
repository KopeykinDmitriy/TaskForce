namespace SCT.Common.Data.Entities
{
    public class TaskTags
    {
        //public required int Id { get; set; }
        public int TaskId { get; set; }
        public int TagId { get; set; }

        public Tasks Task { get; set; }
        public Tag Tag { get; set; }
    }
}
