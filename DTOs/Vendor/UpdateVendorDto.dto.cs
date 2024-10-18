namespace EAD_Backend.DTOs
{
    public class UpdateVendorDto
    {
        public string? VendorName { get; set; }
        public double AverageRanking { get; set; }
        public List<string>? Comments { get; set; }
    }
}
