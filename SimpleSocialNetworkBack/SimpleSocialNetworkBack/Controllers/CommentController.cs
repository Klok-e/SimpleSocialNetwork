using System.Threading.Tasks;
using Business.Models.Requests;
using Business.Services;
using Business.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialNetworkBack.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("/api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateComment([FromBody] CreateCommentModel comment)
        {
            var username = User.Identity.Name!;
            await _commentService.CreateComment(username, comment);
            return Ok();
        }
    }
}