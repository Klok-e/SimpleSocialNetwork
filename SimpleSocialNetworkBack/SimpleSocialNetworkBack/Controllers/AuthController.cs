using System;
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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> Register([FromBody] CredentialsModel cred)
        {
            try
            {
                var registered = await _authService.Register(cred.Login, cred.Password);
                return Ok(registered);
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoggedInUser>> Login([FromBody] CredentialsModel cred)
        {
            try
            {
                var registered = await _authService.Login(cred.Login, cred.Password);
                return Ok(registered);
            }
            catch (ValidationException e)
            {
                return BadRequest();
            }
        }
    }
}