using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class Payment
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("orderId"), BsonRepresentation(BsonType.ObjectId)]
        public required string OrderId { get; set; }

        [BsonElement("customerId"), BsonRepresentation(BsonType.ObjectId)]
        public required string CustomerId { get; set; }

        [BsonElement("productId"), BsonRepresentation(BsonType.ObjectId)]
        public required string ProductId { get; set; } // Reference to the purchased product

        [BsonElement("amount"), BsonRepresentation(BsonType.Double)]
        public required double Amount { get; set; }

        [BsonElement("paymentStatus"), BsonRepresentation(BsonType.String)]
        public required string PaymentStatus { get; set; } // "Completed", "Pending", etc.

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
