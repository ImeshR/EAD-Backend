/*
 * File: Review Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Review management.
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EAD_Backend.Models
{
    public class Review
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("vendorId"), BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } = string.Empty;

        [BsonElement("customerId"), BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("rating"), BsonRepresentation(BsonType.Double)]
        public double Rating { get; set; }

        [BsonElement("comment"), BsonRepresentation(BsonType.String)]
        public string Comment { get; set; } = string.Empty;

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // New property to hold replies
        [BsonElement("replies")]
        public List<Reply> Replies { get; set; } = new List<Reply>();
    }

    // Create a new Reply class
    public class Reply
    {
        [BsonElement("replyId")]
        public string ReplyId { get; set; } = Guid.NewGuid().ToString(); // Automatically generate a new ID

        [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty; // ID of the user who replies

        [BsonElement("content"), BsonRepresentation(BsonType.String)]
        public string Content { get; set; } = string.Empty;

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
