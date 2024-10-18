/*
 * File: Review Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Review management.
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class ShoppingCart
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customerId"), BsonRepresentation(BsonType.ObjectId)]
        public required string CustomerId { get; set; }

        [BsonElement("items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    }

    public class CartItem
    {
        [BsonElement("productId"), BsonRepresentation(BsonType.ObjectId)]
        public required string ProductId { get; set; } 

        [BsonElement("quantity"), BsonRepresentation(BsonType.Int32)]
        public required int Quantity { get; set; } 

        [BsonElement("price"), BsonRepresentation(BsonType.Double)]
        public required double Price { get; set; } 
    }
}
