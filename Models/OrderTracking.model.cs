/*
 * File: Order Tracking Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Order Tracking management.
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class OrderTracking
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.String)]
        public string? TrackingId { get; set; } = Guid.NewGuid().ToString(); 

        [BsonElement("order_id"), BsonRepresentation(BsonType.String)]
        public required string OrderId { get; set; } 

        [BsonElement("current_status"), BsonRepresentation(BsonType.String)]
        public required string CurrentStatus { get; set; } 
        [BsonElement("delivery_estimation"), BsonRepresentation(BsonType.DateTime)]
        public required DateTime DeliveryEstimation { get; set; } 

        [BsonElement("updated_at"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    }
}
