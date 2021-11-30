using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialNetworkBack.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("/api/[controller]")]
    public class OpMessageController : ControllerBase
    {
        private readonly IOpMessageService _opMessageService;

        public OpMessageController(IOpMessageService opMessageService)
        {
            _opMessageService = opMessageService;
        }

        /// <summary>
        ///     Create a new post using provided data
        /// </summary>
        /// <param name="opMessage">Post data</param>
        /// <returns>Id of the created post</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateOpMessage([Required] [FromBody] CreateOpMessageModel opMessage)
        {
            var post = await _opMessageService.MakeAPost(opMessage);
            return Ok(post);
        }

        /// <summary>
        ///     Get a post model with the specified id
        /// </summary>
        /// <param name="id">Id of apost</param>
        /// <returns>Post model</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OpMessageModel>> GetOpMessage(int id)
        {
            return Ok(await _opMessageService.GetById(id));
        }

        /// <summary>
        ///     Get a page of posts
        /// </summary>
        /// <param name="page">Page</param>
        /// <returns>List of posts</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessages([FromQuery] int page = 0)
        {
            return Ok(await _opMessageService.GetAll(page));
        }

        /// <summary>
        ///     Get posts from the specified user
        /// </summary>
        /// <param name="login">User login to get posts from</param>
        /// <returns>Post list</returns>
        [HttpGet("from_user")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessagesFromUser([Required] string login)
        {
            var posts = await _opMessageService.PostsFromUser(login);
            return Ok(posts);
        }

        /// <summary>
        ///     Get comments in a post
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <param name="page">Page</param>
        /// <returns>Comment list</returns>
        [HttpGet("comments/{postId}/{page}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments(int postId, int page)
        {
            return Ok(await _opMessageService.GetComments(postId, page));
        }

        /// <summary>
        ///     Check whether the specified post exists
        /// </summary>
        /// <param name="postId">Id of a post</param>
        /// <returns>A bool indicating whether the post exists</returns>
        [HttpGet("exists/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> PostExists(int postId)
        {
            var exists = await _opMessageService.PostExists(postId);
            return Ok(exists);
        }

        /// <summary>
        ///     Vote on a post
        /// </summary>
        /// <param name="vote">Vote data</param>
        [HttpPost("vote")]
        [Authorize]
        public async Task<ActionResult> VotePost([Required] [FromBody] VotePost vote)
        {
            await _opMessageService.VotePost(vote);
            return Ok();
        }

        /// <summary>
        ///     Delete the specifid post
        /// </summary>
        /// <param name="postId">Id of a post</param>
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePost([Required] int postId)
        {
            await _opMessageService.DeletePostSoft(postId);
            return Ok();
        }
    }
}
