using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostsController : ControllerBase
  {
    private readonly DataContext context;

    public PostsController(DataContext context)
    {
      this.context = context;
    }

/// <summary>
/// Get api/posts
/// </summary>
/// <returns>A list of posts</returns>
    [HttpGet]
    public ActionResult<List<Post>> Get()
    {
      return this.context.Posts.ToList();
    }

/// <summary>
/// GET api/post/[id]
/// </summary>
/// <param name="id">Post id</param>
/// <returns>A Single Post</returns>
    [HttpGet("{id}")]
    public ActionResult<Post> GetById(Guid id) {
      return this.context.Posts.Find(id);
    }
/// <summary>
/// Post api/post
/// </summary>
/// <param name="request">Json request containing post fields</param>
/// <returns>A new post</returns>
    [HttpPost]
    public ActionResult<Post> Create([FromBody]Post request) {
      var post = new Post 
      {
        Id = request.Id,
        Title = request.Title,
        Body = request.Body,
        Date = request.Date
      };

      context.Posts.Add(post);
      var success = context.SaveChanges() > 0;

      if (success) {
        return post;
      }

      throw new Exception("Error creating post");
    }

/// <summary>
/// Put api/put
/// </summary>
/// <param name="request">Json request containing an existing field</param>
/// <returns>An updated post</returns>
    [HttpPut]
    public ActionResult<Post> Update([FromBody]Post request) {
      var post = context.Posts.Find(request.Id);

      if (post == null) {
        throw new Exception("Could not find post");
      }

      post.Title = request.Title != null ? request.Title : post.Title;
      post.Body = request.Body != null ? request.Body : post.Body;
      post.Date = request.Date != null ? request.Date : post.Date;

      var success = context.SaveChanges() > 0;

      if (success) {
        return post;
      }

      throw new Exception("Error updating post");
    }
    
 /// <summary>
 /// Delete api/Post/[id]
 /// </summary>
 /// <param name="id">post id</param>
 /// <returns>true if success full</returns>
    [HttpDelete("{id}")]
    public ActionResult<bool> Delete(Guid id) {
      var post = context.Posts.Find(id);

      if (post == null) {
        throw new Exception("Could not find post.");
      }

      context.Remove(post);

      var success = context.SaveChanges() > 0;

      if (success) {
        return true;
      }

      throw new Exception("Error deleting post.");
    }
  }
}
