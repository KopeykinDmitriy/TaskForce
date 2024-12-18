namespace SCT.Common.Data.Entities
{
    public class UserProject
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        // связи
        public Project Project { get; set; }
        public User User { get; set; }

    }
}