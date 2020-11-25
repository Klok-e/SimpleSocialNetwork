using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Common;
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

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LimitedUserModel>>> SearchUsers([FromQuery] SearchUsersModel search)
        {
            var users = await _userService.SearchUsers(search);
            return Ok(users);
        }

        [HttpPut("elevate")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> ElevateUser([Required] string login)
        {
            await _userService.ElevateUser(login);
            return Ok();
        }

        [HttpPost("ban")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> BanUser([Required] BanUserModel ban)
        {
            await _userService.BanUser(ban);
            return Ok();
        }

        [HttpPost("unban")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> LiftUserBan([Required] string user)
        {
            await _userService.LiftBanFromUser(user);
            return Ok();
        }

        /// <summary>
        ///     Soft delete user
        ///     Authorized: either user themselves or an admin
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteUser([Required] string login)
        {
            await _userService.DeleteUserSoft(login);
            return Ok();
        }

        [HttpGet("banned")]
        [Authorize]
        public async Task<ActionResult<bool>> UserBanned([Required] string login)
        {
            var banned = await _userService.UserBanned(login);
            return Ok(banned);
        }
    }
}