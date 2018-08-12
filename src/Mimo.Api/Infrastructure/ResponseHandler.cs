using Microsoft.AspNetCore.Mvc;
using Mimo.Api.Dtos;
using System.Net;

namespace Mimo.Api.Infrastructure
{
    public class ResponseHandler : IResponseHandler
    {
        public ActionResult<T> GetResponse<T>(Result<T> result)
        {
            if (result.Success)
            {
                return new OkObjectResult(result.Value);
            }
            return GetErrorResponse(result);
        }

        public ActionResult GetSimpleResponse<T>(Result<T> result)
        {
            if (result.Success)
            {
                return new OkResult();
            }
            return GetErrorResponse(result);
        }

        public ActionResult<T> GetCreatedResponse<T>(Result<T> result, string route, object routeParams)
        {
            if (result.Success)
            {
                return new CreatedAtRouteResult(route, routeParams, result.Value);
            }
            return GetErrorResponse(result);
        }

        public ActionResult GetErrorResponse(Result result)
        {
            var errorResult = new ObjectResult(result.Errors)
            {
                StatusCode = (int)(result.StatusCode ?? HttpStatusCode.BadRequest)
            };

            return errorResult;
        }
    }
}
