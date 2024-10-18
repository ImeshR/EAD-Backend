namespace EAD_Backend.DTOs
{
    public class UpdateOrderDto
    {
        public double? TotalAmount { get; set; } 
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
    }
}
