using Microsoft.AspNetCore.Mvc;
using Mimo.Api.Dtos;

namespace Mimo.Api.Infrastructure
{
    public interface IResponseHandler
    {
        ActionResult GetSimpleResponse<T>(Result<T> result);
        ActionResult<T> GetResponse<T>(Result<T> result);
        ActionResult<T> GetCreatedResponse<T>(Result<T> result, string route, object routeParams);
        ActionResult GetErrorResponse(Result result);
    }
}