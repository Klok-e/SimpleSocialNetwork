using System;
using System.Collections.Generic;
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
        private readonly IUserService _userService;

        public OpMessageController(IOpMessageService opMessageService, IUserService userService)
        {
            _opMessageService = opMessageService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateOpMessage([FromBody] CreateOpMessageModel opMessage)
        {
            var username = User.Identity.Name!;
            try
            {
                var post = await _opMessageService.MakeAPost(username, opMessage);
                return Ok(post);
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OpMessageModel>> GetOpMessage(int id)
        {
            try
            {
                return Ok(await _opMessageService.GetById(id));
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessages()
        {
            try
            {
                return Ok(await _opMessageService.GetAll());
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }
    }
}