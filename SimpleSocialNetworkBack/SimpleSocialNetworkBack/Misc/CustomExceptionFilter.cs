using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleSocialNetworkBack.Misc
{
    // TODO: come up with a better name
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException _:
                    context.Result = new BadRequestResult();
                    break;
                case BadCredentialsException _:
                    context.Result = new UnauthorizedResult();
                    break;
                case ForbiddenException _:
                    context.Result = new ForbidResult();
                    break;
            }
        }
    }
}