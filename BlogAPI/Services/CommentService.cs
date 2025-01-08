using BlogAPI.Models;
using MongoDB.Driver;

namespace BlogAPI.Services;

public class CommentService: ICommentService
{
    private readonly IMongoCollection<Comment> _comments;
    
     public CommentService(IMongoDBDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _comments = database.GetCollection<Comment>("Comments");
    }

    public List<Comment> GetCommentsByPostId(string postId)
    {
        return _comments.Find(com => com.PostId == postId).ToList();
    }

    public Comment CreateComment(Comment comment)
    {
        _comments.InsertOne(comment);
        return comment;
    }

    public Comment GetCommentById(string id)
    {
        return _comments.Find(com => com.Id == id).FirstOrDefault();
    }

    public void UpdateComment(string id, Comment comment)
    {
        _comments.ReplaceOne(com => com.Id == id, comment);
    }

    public void deleteComment(string id)
    {
        _comments.DeleteOne(com => com.Id == id);
    }

    public void DeleteCommentByPostId(string postId)
    {
        _comments.DeleteMany(com => com.PostId == postId);
    }
}
