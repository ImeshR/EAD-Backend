namespace EAD_Backend.DTOs
{
    public class UpdateNotificationDto
    {
        public string? UserId { get; set; }

        public string? Message { get; set; }

        public string? Type { get; set; }

        public bool ReadStatus { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
