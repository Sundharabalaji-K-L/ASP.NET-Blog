using BlogAPI.Models;

namespace BlogAPI.Services;

public interface IPostService
{
    List<Post> GetPosts();
    Post GetPostById(string id);
    Post CreatePost(Post post);
    void UpdatePost(string id, Post post);
    void DeletePost(string id);
}