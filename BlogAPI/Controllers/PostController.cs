using BlogDataLibrary;
using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ISqlData _db;

        public PostController(ISqlData db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ListPosts()
        {
            var posts = _db.ListPosts();
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult ShowPostDetails(int id)
        {
            var post = _db.ShowPostDetails(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        private int GetCurrentUserId()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                string id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (id != null)
                {
                    return Convert.ToInt32(id);
                }
            }

            // Fallback to User ID 1 for testing
            return 1;
        }

        // [Authorize]
        [HttpPost]
        [Route("/api/[controller]/add")]
        public ActionResult AddPost([FromBody] PostForm form)
        {
            PostModel post = new PostModel();
            post.Title = form.Title;
            post.Body = form.Body;
            post.DateCreated = DateTime.Now;
            post.UserId = GetCurrentUserId();

            _db.AddPost(post);

            return Ok("Post created.");
        }
    }
}