using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.Requests;
using Business.Models.Responses;
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
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessages()
        {
            return Ok(await _opMessageService.GetAll());
        }

        [HttpGet("from_user")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessagesFromUser([Required] string login)
        {
            var posts = await _opMessageService.PostsFromUser(login);
            return Ok(posts);
        }

        [HttpGet("comments/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments(int postId)
        {
            return Ok(await _opMessageService.GetComments(postId));
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
    }
}