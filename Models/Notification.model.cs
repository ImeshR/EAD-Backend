/*
 * File: Notification Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Notification management.
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class Notification
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [BsonElement("message"), BsonRepresentation(BsonType.String)]
        public required string Message { get; set; }

        [BsonElement("type"), BsonRepresentation(BsonType.String)]
        public required string Type { get; set; }

        [BsonElement("readStatus"), BsonRepresentation(BsonType.Boolean)]
        public bool ReadStatus { get; set; } = false;

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
