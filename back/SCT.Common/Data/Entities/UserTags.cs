namespace SCT.Common.Data.Entities
{
    public class UserTags
    {
        public int Id {  get; set; }
        public required int UserId { get; set; }
        public required int TagId { get; set; }
        public string? TypeAccessTag { get; set; } = string.Empty; // Что это такое, я забыл :(

        public required User User { get; set; }
        public required Tag Tag { get; set; }
    }
}
