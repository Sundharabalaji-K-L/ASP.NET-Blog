using BlogAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogAPI.Services;

public class PostService: IPostService
{
    private readonly IMongoCollection<Post> _posts;
    
    public PostService(IMongoDBDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _posts = database.GetCollection<Post>(settings.PostCollectionName);

    }

    public List<Post> GetPosts()
    {
        SortDefinition<Post> definition = new BsonDocument { { "updated_at",  -1 } };
        return _posts.Find(post => true).Sort(definition).ToList();
    }

    public Post GetPostById(string id)
    {
        return _posts.Find(post => post.Id == id).FirstOrDefault();
    }

    public Post CreatePost(Post post)
    {
       _posts.InsertOne(post);
       return post;
    }

    public void UpdatePost(string id, Post post)
    {
        _posts.ReplaceOne(p => p.Id == id, post);
    }

    public void DeletePost(string id)
    {
        _posts.DeleteOne(p => p.Id == id);
    }
}