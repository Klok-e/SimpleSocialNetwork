using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.Models.Responses;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialNetworkBack.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("/api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("subscribed_to")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubscriptionModel>>> GetUserSubscribedTo([Required] string login)
        {
            var subs = await _subscriptionService.GetUserSubscribedTo(login);
            return Ok(subs);
        }

        [HttpGet("is_subscribed_to")]
        [Authorize]
        public async Task<ActionResult<bool>> IsUserSubscribedTo([Required] string login)
        {
            var isSub = await _subscriptionService.IsUserSubscribedTo(login);
            return Ok(isSub);
        }

        [HttpPost("sub")]
        [Authorize]
        public async Task<ActionResult> Subscribe([Required] string login)
        {
            await _subscriptionService.SubscribeTo(login);
            return Ok();
        }

        [HttpPost("unsub")]
        [Authorize]
        public async Task<ActionResult> Unsubscribe([Required] string login)
        {
            await _subscriptionService.UnsubscribeFrom(login);
            return Ok();
        }
    }
}