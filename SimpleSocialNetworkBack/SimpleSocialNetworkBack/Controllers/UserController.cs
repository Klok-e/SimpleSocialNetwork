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

        /// <summary>
        /// Check whether the specified user exists (deleted users also regarded as existing)
        /// </summary>
        /// <param name="login">The specified user's login</param>
        /// <returns>Whether exists</returns>
        [HttpGet("exists")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> UserExists([Required] string login)
        {
            var exists = await _userService.UserExists(login);
            return Ok(exists);
        }

        /// <summary>
        /// Check whether the specified user was deleted
        /// </summary>
        /// <param name="login">The specified user's login</param>
        /// <returns>Whether deleted</returns>
        [HttpGet("deleted")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> UserDeleted([Required] string login)
        {
            var exists = await _userService.UserDeleted(login);
            return Ok(exists);
        }

        /// <summary>
        /// Get a full model of the specified user
        /// </summary>
        /// <param name="login">Login of a user</param>
        /// <returns>Full model</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetUser([Required] string login)
        {
            var user = await _userService.GetUser(login);
            return Ok(user);
        }

        /// <summary>
        /// Change user info
        /// </summary>
        /// <param name="changeInfo">New user info</param>
        [HttpPut("info")]
        [Authorize]
        public async Task<ActionResult> ChangeUserInfo([Required] ChangeUserInfo changeInfo)
        {
            await _userService.ChangeUserInfo(changeInfo);
            return Ok();
        }

        /// <summary>
        /// Get a limited model of the specified user
        /// </summary>
        /// <param name="login">Login of a user</param>
        /// <returns>Limited model</returns>
        [HttpGet("limited")]
        [AllowAnonymous]
        public async Task<ActionResult<LimitedUserModel>> GetUserLimited([Required] string login)
        {
            var user = await _userService.GetUserLimited(login);
            return Ok(user);
        }

        /// <summary>
        /// Search users
        /// </summary>
        /// <param name="search">Parameters to search for</param>
        /// <returns>List of matching limited user models</returns>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LimitedUserModel>>> SearchUsers([FromQuery] SearchUsersModel search)
        {
            var users = await _userService.SearchUsers(search);
            return Ok(users);
        }

        /// <summary>
        /// Elevate the specified user to admin
        /// </summary>
        /// <param name="login">Login of a user</param>
        [HttpPut("elevate")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> ElevateUser([Required] string login)
        {
            await _userService.ElevateUser(login);
            return Ok();
        }

        /// <summary>
        /// Ban the specified user
        /// </summary>
        /// <param name="ban">Ban data</param>
        [HttpPost("ban")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> BanUser([Required] BanUserModel ban)
        {
            await _userService.BanUser(ban);
            return Ok();
        }

        /// <summary>
        /// Lift all current bans of the specified user 
        /// </summary>
        /// <param name="user">Login of a user</param>
        [HttpPost("unban")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> LiftUserBan([Required] string user)
        {
            await _userService.LiftBanFromUser(user);
            return Ok();
        }

        /// <summary>
        /// Soft delete user;
        /// Authorized: either user themselves or an admin
        /// </summary>
        /// <param name="login">Login of a user</param>
        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> DeleteUser([Required] string login)
        {
            await _userService.DeleteUserSoft(login);
            return Ok();
        }

        /// <summary>
        /// Check whether user is banned
        /// </summary>
        /// <param name="login">Login of a user</param>
        /// <returns>Whether user is banned</returns>
        [HttpGet("banned")]
        [Authorize]
        public async Task<ActionResult<bool>> UserBanned([Required] string login)
        {
            var banned = await _userService.UserBanned(login);
            return Ok(banned);
        }
    }
}