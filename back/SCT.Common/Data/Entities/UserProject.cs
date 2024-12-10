namespace SCT.Common.Data.Entities
{
    public class UserProject // пока пусть будет, но не факт, что сущность будет использоваться
    {
        //public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        // связи
        public required Project Project { get; set; }
        public required User User { get; set; }

    }
}