namespace EAD_Backend.DTOs
{
    public class SelfRegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
       
    }
}