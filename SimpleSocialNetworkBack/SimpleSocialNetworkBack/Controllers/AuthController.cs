using System;
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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
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