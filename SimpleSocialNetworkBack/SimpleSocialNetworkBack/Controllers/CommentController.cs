using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Models.Requests;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> CreateComment([Required] [FromBody] CreateCommentModel comment)
        {
            await _commentService.CreateComment(comment);
            return Ok();
        }

        [HttpPost("vote")]
        [Authorize]
        public async Task<ActionResult> VoteComment([Required] [FromBody] VoteComment vote)
        {
            await _commentService.VoteComment(vote);
            return Ok();
        }
    }
}