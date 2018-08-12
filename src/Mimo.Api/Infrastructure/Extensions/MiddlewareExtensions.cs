using Microsoft.AspNetCore.Builder;
using Mimo.Api.Infrastructure.Middlewares;

namespace Mimo.Api.Infrastructure.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

    }
}
