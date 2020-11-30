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

        /// <summary>
        /// Get all users the specified user is subscribed to
        /// </summary>
        /// <param name="login">Login of a user</param>
        /// <returns>List of subscriptions</returns>
        [HttpGet("subscribed_to")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SubscriptionModel>>> GetUserSubscribedTo([Required] string login)
        {
            var subs = await _subscriptionService.GetUserSubscribedTo(login);
            return Ok(subs);
        }

        /// <summary>
        /// Check whether current user is subscribed to the specified user
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Bool indicating whether current user is subscribed to the specified user</returns>
        [HttpGet("is_subscribed_to")]
        [Authorize]
        public async Task<ActionResult<bool>> IsUserSubscribedTo([Required] string login)
        {
            var isSub = await _subscriptionService.IsUserSubscribedTo(login);
            return Ok(isSub);
        }

        /// <summary>
        /// Subscribe current user to the specified user
        /// </summary>
        /// <param name="login">Specified user's login</param>
        [HttpPost("sub")]
        [Authorize]
        public async Task<ActionResult> Subscribe([Required] string login)
        {
            await _subscriptionService.SubscribeTo(login);
            return Ok();
        }

        /// <summary>
        /// Unsubscribe current user from the specified user
        /// </summary>
        /// <param name="login">Specified user's login</param>
        [HttpPost("unsub")]
        [Authorize]
        public async Task<ActionResult> Unsubscribe([Required] string login)
        {
            await _subscriptionService.UnsubscribeFrom(login);
            return Ok();
        }
    }
}