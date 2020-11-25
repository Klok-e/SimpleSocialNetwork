using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SimpleSocialNetworkBack.Misc
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterDescriptors.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterDescriptors.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);

            //TODO: fix this
            //if (!isAuthorized || allowAnonymous) return;

            // if (operation.Parameters == null)
            //     operation.Parameters = new List<OpenApiParameter>();
            // operation.Parameters.Add(new OpenApiParameter()
            // {
            //     Name = "X-Authorization",
            //     Description = "access token",
            //     In = ParameterLocation.Header,
            //     Required = false,
            //     Schema = new OpenApiSchema {Type = "String", Default = new OpenApiString("Bearer {access token}")}
            // });
        }
    }
}