using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogAPI.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = string.Empty;
    
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;
    
    [BsonElement("author")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Author { get; set; } = string.Empty;
    
    [BsonElement("post_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = string.Empty;
    
    [BsonElement("created_at")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("modified_at")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime ModifiedAt { get; set; }
}