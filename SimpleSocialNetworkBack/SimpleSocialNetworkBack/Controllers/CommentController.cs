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

        /// <summary>
        /// Create a new comment in specified post
        /// </summary>
        /// <param name="comment">Comment data</param>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateComment([Required] [FromBody] CreateCommentModel comment)
        {
            await _commentService.CreateComment(comment);
            return Ok();
        }

        /// <summary>
        /// Vote on a comment
        /// </summary>
        /// <param name="vote">Vote data</param>
        [HttpPost("vote")]
        [Authorize]
        public async Task<ActionResult> VoteComment([Required] [FromBody] VoteComment vote)
        {
            await _commentService.VoteComment(vote);
            return Ok();
        }

        /// <summary>
        /// Soft delete comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteComment([Required] [FromQuery] CommentKeyModel commentId)
        {
            await _commentService.DeleteCommentSoft(commentId);
            return Ok();
        }
    }
}