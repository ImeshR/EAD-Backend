namespace EAD_Backend.DTOs
{
    public class UpdateUserDto
    {
        public required string Name { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }

    }
}