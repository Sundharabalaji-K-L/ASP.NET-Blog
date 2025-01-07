﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogAPI.Models;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    [BsonElement("hashed_password")]
    public string PasswordHash { get; set; } = string.Empty;
}
