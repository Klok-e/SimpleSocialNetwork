using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ValidationException = Business.Validation.ValidationException;

namespace SimpleSocialNetworkBack.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("exists")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> UserExists([Required] string login)
        {
            var exists = await _userService.UserExists(login);
            return Ok(exists);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetUser([Required] string login)
        {
            var user = await _userService.GetUser(login);
            return Ok(user);
        }

        [HttpPut("info")]
        [Authorize]
        public async Task<ActionResult> ChangeUserInfo([Required] ChangeUserInfo changeInfo)
        {
            await _userService.ChangeUserInfo(changeInfo);
            return Ok();
        }

        [HttpGet("limited")]
        [AllowAnonymous]
        public async Task<ActionResult<LimitedUserModel>> GetUserLimited([Required] string login)
        {
            var user = await _userService.GetUserLimited(login);
            return Ok(user);
        }
    }
}