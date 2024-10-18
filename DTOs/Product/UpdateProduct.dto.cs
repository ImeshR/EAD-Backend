namespace EAD_Backend.DTOs
{
    public class UpdateProductDto
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public double Price { get; set; }
        public string[]? Images { get; set; }
        public bool Active { get; set; }
        public int StockCount { get; set; }

    }
}