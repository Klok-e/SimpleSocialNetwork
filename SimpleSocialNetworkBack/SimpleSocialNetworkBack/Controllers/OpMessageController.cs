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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateOpMessage([Required] [FromBody] CreateOpMessageModel opMessage)
        {
            var post = await _opMessageService.MakeAPost(opMessage);
            return Ok(post);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OpMessageModel>> GetOpMessage(int id)
        {
            return Ok(await _opMessageService.GetById(id));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessages([FromQuery] int page = 0)
        {
            return Ok(await _opMessageService.GetAll(page));
        }

        [HttpGet("from_user")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessagesFromUser([Required] string login)
        {
            var posts = await _opMessageService.PostsFromUser(login);
            return Ok(posts);
        }

        [HttpGet("comments/{postId}/{page}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments(int postId, int page)
        {
            return Ok(await _opMessageService.GetComments(postId, page));
        }

        [HttpGet("exists/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> PostExists(int postId)
        {
            var exists = await _opMessageService.PostExists(postId);
            return Ok(exists);
        }

        [HttpPost("vote")]
        [Authorize]
        public async Task<ActionResult> VotePost([Required] [FromBody] VotePost vote)
        {
            await _opMessageService.VotePost(vote);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePost([Required] int postId)
        {
            await _opMessageService.DeletePostSoft(postId);
            return Ok();
        }
    }
}