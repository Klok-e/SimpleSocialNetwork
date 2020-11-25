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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register([Required] [FromBody] CredentialsModel cred)
        {
            var registered = await _authService.Register(cred.Login, cred.Password);
            return Ok(registered);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedInUser>> Login([Required] [FromBody] CredentialsModel cred)
        {
            var registered = await _authService.Login(cred.Login, cred.Password);
            return Ok(registered);
        }
    }
}