namespace SCT.Users.DTOs
{
    public class UserDto
    {
        public required string Name { get; set; }
        public string? Surname {get; set;}
        public string? Email { get; set; }
        public required string Role { get; set; }
    }
}