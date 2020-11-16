using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Models.Requests;
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


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UserExists([Required] string login)
        {
            try
            {
                var exists = await _userService.UserExists(login);
                return Ok(exists);
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }
    }
}