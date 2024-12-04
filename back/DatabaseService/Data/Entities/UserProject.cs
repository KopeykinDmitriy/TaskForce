namespace DatabaseService.Data.Entities
{
    public class UserProject // пока пусть будет, но не факт, что сущность будет использоваться
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }


    }
}