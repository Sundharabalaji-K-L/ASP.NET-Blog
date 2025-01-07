using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogAPI.Models;

[BsonIgnoreExtraElements]
public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;
    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;
    [BsonElement("author")]
    public string Author { get; set; } = string.Empty;
    [BsonElement("created_at")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [BsonElement("updated_at")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
}
