using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Business.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialNetworkBack.Controllers
{
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
        public async Task<ActionResult<OpMessageModel>> CreateOpMessage([FromBody] CreateOpMessageModel opMessage)
        {
            var username = User.Identity.Name!;
            try
            {
                var user = await _userService.Get(username);
                var post = await _opMessageService.MakeAPost(user, opMessage);
                return Ok(post);
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [AllowAnonymous]
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