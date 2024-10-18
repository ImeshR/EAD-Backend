/*
 * File: Product Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Product management.
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class Product
    {

        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public required string Description { get; set; }

        [BsonElement("categoryId"), BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }


        [BsonElement("price"), BsonRepresentation(BsonType.Double)]
        public required double Price { get; set; }

        [BsonElement("images")]
        public string[] Images { get; set; } = Array.Empty<string>();

        [BsonElement("active"), BsonRepresentation(BsonType.Boolean)]
        public bool? Active { get; set; }

        [BsonElement("stockCount"), BsonRepresentation(BsonType.Int32)]
        public required int StockCount { get; set; }

        [BsonElement("vendorId"), BsonRepresentation(BsonType.ObjectId)]
        public string? VendorId { get; set; }

        [BsonElement("createdAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt"), BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // New properties for ratings
        [BsonElement("averageRating"), BsonRepresentation(BsonType.Double)]
        public double AverageRating { get; set; } = 0.0;

        [BsonElement("ratings")]
        public List<double> Ratings { get; set; } = new List<double>();

    }
}