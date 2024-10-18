/*
 * File: Category Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for Categoty management.
*/

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class Category
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }



        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string? Description { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public required string Status { get; set; }
    }
}
