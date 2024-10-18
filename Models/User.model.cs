/*
 * File: User Model
 * Author: Fernando B.K.M.
 * Description: This file contains the attributes for User management.
*/
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAD_Backend.Models
{
    public class User
    {

        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public required string Name { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public required string Email { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public required string Password { get; set; }

        [BsonElement("phomeNumber"), BsonRepresentation(BsonType.String)]
        public string PhoneNumber { get; set; }

        [BsonElement("address"), BsonRepresentation(BsonType.String)]
        public string Address { get; set; }

        [BsonElement("role"), BsonRepresentation(BsonType.ObjectId)]
        public required string Role { get; set; }

        [BsonElement("active"), BsonRepresentation(BsonType.Boolean)]
        public bool? Active { get; set; }

        
        
    }
}