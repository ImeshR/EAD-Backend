/*
 * File: Inventory Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Inventory management.
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class Inventory
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("productId"), BsonRepresentation(BsonType.ObjectId)]
        public string? ProductId { get; set; } 

        [BsonElement("vendorId"), BsonRepresentation(BsonType.ObjectId)]
        public string? VendorId { get; set; } 

        [BsonElement("quantity_available"), BsonRepresentation(BsonType.Int32)]
        public int QuantityAvailable { get; set; }

        [BsonElement("low_stock_alert"), BsonRepresentation(BsonType.Boolean)]
        public bool LowStockAlert { get; set; } 

        [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        [BsonElement("updated_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    }
}
