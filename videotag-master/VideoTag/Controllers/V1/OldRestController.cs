using System.Net;
using AryDotNet.Messaging;
using Microsoft.AspNetCore.Mvc;
using AryDotNet.Result;
using MediatR;


namespace VideoTag.Controllers.V1;


public record ApiResult(bool Success, string? Error);

public record ApiResult<T>(bool Success, T? Data, string? Error);

public abstract class OldRestController() : Controller
{
    protected IActionResult
        HandleResult(Result<MessagingError> result, HttpStatusCode successCode = HttpStatusCode.OK) =>
        result.MapOrElse(() => StatusCode((int)successCode, new ApiResult(true, null)), HandleError);

    protected IActionResult HandleResult<T>(Result<T, MessagingError> result,
        HttpStatusCode successCode = HttpStatusCode.OK) =>
        result.MapOrElse<IActionResult>((value) => StatusCode((int)successCode, new ApiResult<T>(true, value, null)),
            HandleError<T>);

    protected IActionResult HandleError(MessagingError? error)
    {
        var errorResult = new ApiResult(false, error?.Message);
        return StatusCode((int)(error?.Code ?? HttpStatusCode.InternalServerError), errorResult);
    }

    protected IActionResult HandleError<T>(MessagingError? error)
    {
        var errorResult = new ApiResult<T>(false, default, error?.Message);
        return StatusCode((int)(error?.Code ?? HttpStatusCode.InternalServerError), errorResult);
    }
}