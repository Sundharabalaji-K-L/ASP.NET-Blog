using BlogAPI.Models;

namespace BlogAPI.Services;

public interface ICommentService
{
    Comment CreateComment(Comment comment);
    List<Comment> GetCommentsByPostId(string postId);
    Comment GetCommentById(string id);
    void UpdateComment(string id, Comment comment);
    void deleteComment(string id);

    void DeleteCommentByPostId(string postId);
}