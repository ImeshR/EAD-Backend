using EAD_Backend.Models;

public class OrderItemDto
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public string OrderStatus { get; set; }
    public OrderItem Item { get; set; } // Assuming OrderItem is a class with relevant properties.
}
