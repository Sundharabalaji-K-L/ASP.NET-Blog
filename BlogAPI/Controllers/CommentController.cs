using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController: ControllerBase
{
    
    private readonly ICommentService _commentService;
    private readonly IPostService _postService;
    
    public CommentController(ICommentService commentService, IPostService postService)
    {
        _commentService = commentService;
        _postService = postService;
    }
    
    // GET /api/Comment/post/{post_id}
    [HttpGet("/api/Comment/post/{id}")]
    public ActionResult<List<Comment>> GetComments(string id)
    {
        var existingPost = _postService.GetPostById(id);
        if (existingPost == null)
        {
            return NotFound($"Post with id: {id} does not exist");
        }

        return _commentService.GetCommentsByPostId(id);
    }
    
    // GET: /api/Comment/{comment_id}
    [HttpGet("{id}")]
    public ActionResult<Comment> GetComment(string id)
    {
        var existingComment = _commentService.GetCommentById(id);
        if (existingComment == null)
        {
            return NotFound($"Comment with id: {id} does not exist");
        }

        return Ok(existingComment);
    }
    
    // POST: /api/Comment
    [HttpPost("{id}"), Authorize]
    public ActionResult<Comment> AddComment(string id, [FromBody] CommentRequest comment)
    {
        var userId = User.FindFirst("Id")?.Value;
        var existingPost = _postService.GetPostById(id);

        if (existingPost == null)
        {
            return NotFound($"post with id {id} does not exist");
        }

        Comment newComment = new Comment
        {
            Content = comment.Content,
            PostId = id,
            Author = userId,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        _commentService.CreateComment(newComment);
        return Ok(newComment);
    }

    [HttpPut("{id}"), Authorize]
    public ActionResult UpdateComment(string id, [FromBody] CommentRequest comment)
    {
        var userId = User.FindFirst("Id")?.Value;
        var existingComment = _commentService.GetCommentById(id);
        
        if (existingComment == null)
        {
            return NotFound($"Comment with id: {id} does not exist");
        }

        if (existingComment.Author != userId)
        {
            return Unauthorized($"You are not authorized to update this comment");
        }
        
        existingComment.Content = comment.Content;
        existingComment.ModifiedAt = DateTime.Now;
        _commentService.UpdateComment(id, existingComment);
        return NoContent();
    }

    [HttpDelete("{id}"), Authorize]
    public ActionResult DeleteComment(string id)
    {
        var userId = User.FindFirst("Id")?.Value;
        var existingComment = _commentService.GetCommentById(id);

        if (existingComment == null)
        {
            return NotFound($"Comment with id: {id} does not exist");
        }

        if (existingComment.Author != userId)
        {
            return Unauthorized($"You are not authorized to delete this comment");
        }
        
        _commentService.deleteComment(id);
        return NoContent();
    }    
}