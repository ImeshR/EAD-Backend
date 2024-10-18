/*
 * File: Order Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Order management.
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class OrderItem
    {
        [BsonElement("productId"), BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = string.Empty;


        [BsonElement("vendorId"), BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } = string.Empty;

        [BsonElement("quantity"), BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }

        [BsonElement("priceAtPurchase"), BsonRepresentation(BsonType.Double)]
        public double PriceAtPurchase { get; set; }
    }

    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customerId"), BsonRepresentation(BsonType.ObjectId)]
        public string? CustomerId { get; set; }

        [BsonElement("totalAmount"), BsonRepresentation(BsonType.Double)]
        public required double TotalAmount { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string Status { get; set; } = "Pending";  // Default status

        [BsonElement("orderItems")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
