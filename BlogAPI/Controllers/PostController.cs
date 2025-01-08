using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController: ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    public PostController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;   
    }
    
    // GET: api/post
    [HttpGet]
    public ActionResult<List<Post>> Get()
    {
        return _postService.GetPosts();
    }
    
    // GET: api/Post/id
    [HttpGet("{id}")]
    public ActionResult<Post> Get(string id)
    {
        var post = _postService.GetPostById(id);
        if (post == null)
        {
            return NotFound($"No Post Found with id: {id}");
        }
        return _postService.GetPostById(id);
    }

   // POST: api/Post
   //[HttpPost, Authorize(Roles = "Admin")]
   [HttpPost, Authorize]
   public ActionResult<Post> Post([FromBody] PostRequest postR)
   {
       var userId = User.FindFirst("Id")?.Value;
       
       var post = new Post
       {
           Title = postR.Title,
           Content = postR.Content,
           CreatedAt = DateTime.Now,
           ModifiedAt = DateTime.Now,
           Author = userId
       };   
       
       _postService.CreatePost(post);
       return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
   }
   
   // PUT: api/Post/id
   [HttpPut("{id}"), Authorize]
   public ActionResult Put(string id, PostRequest postR)
   {
       var userId = User.FindFirst("Id")?.Value;
       var existingPost = _postService.GetPostById(id);
       if (existingPost == null)
       {
           return NotFound($"No Post Found with id: {id}");
       }

       if (existingPost.Author != userId)
       {
            return Unauthorized("You are not authorized to update this post");    
       }
       
       existingPost.Title = postR.Title;
       existingPost.Content = postR.Content;
       existingPost.ModifiedAt = DateTime.Now;
       _postService.UpdatePost(id, existingPost);
       return NoContent();
   }
   
   // DELETE: /api/Post/id
   [HttpDelete("{id}"), Authorize]
   public ActionResult Delete(string id)
   {
       var UserId = User.FindFirst("Id")?.Value;
       var existingPost = _postService.GetPostById(id);
       if (existingPost == null)
       {
           return NotFound($"No Post Found with id: {id}");
       }

       if (existingPost.Author != UserId)
       {
           return Unauthorized("You are not authorized to delete this post");
       }
       _postService.DeletePost(id);
       _commentService.DeleteCommentByPostId(id);
       return Ok($"Post with id: {id} has been deleted");
   }
}