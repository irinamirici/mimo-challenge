using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Infrastructure
{
    public class AddSwaggerAuthorizeHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerAuthenticationAttributes = context.ApiDescription.ActionDescriptor.FilterDescriptors
                .FirstOrDefault(x=>x.Filter.ToString() == "Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter" );
                //.OfType<AuthorizeFilter>().ToList();
            if (controllerAuthenticationAttributes != null )
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "string",
                    Required = true
                });
            }
        }
    }
}
