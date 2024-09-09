using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Helpers;

public static class ExceptionHelper
{
    public static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };
}

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode = httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        if (exception is NotImplementedException)
        {
            return false;
        }

        ErrorModel model = new()
        {
            Header = new ErrorHeader
            {
                ResultCode = statusCode.ToString(),
                ResultMessage = $"{HttpHelper.GetCodeMessage(statusCode)} : {exception.Message}"
            },
            Body = new ErrorBody
            {
                ResultMessageDetail = $"{httpContext.Request.Host.Value}{httpContext.Request.Path} : {await HttpHelper.GetRequestInfo(httpContext)}"
            }
        };

        LogHelper.Logger.Fatal($"WebAPI Global Error(AppExceptionHandler) : {exception.Message}");
        await httpContext.Response.WriteAsJsonAsync(model, ExceptionHelper.Options, cancellationToken);
        return true;
    }
}

public class AppNotImplementedExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode = httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;
        httpContext.Response.ContentType = "application/json";

        if (exception is not NotImplementedException)
        {
            return false;
        }

        ErrorModel model = new()
        {
            Header = new ErrorHeader
            {
                ResultCode = statusCode.ToString(),
                ResultMessage = $"{HttpHelper.GetCodeMessage(statusCode)} : {exception.Message}"
            },
            Body = new ErrorBody
            {
                ResultMessageDetail = $"{httpContext.Request.Host.Value}{httpContext.Request.Path} : {await HttpHelper.GetRequestInfo(httpContext)}"
            }
        };

        LogHelper.Logger.Fatal($"WebAPI Global Error(AppNotImplementedExceptionHandler) : {exception.Message}");
        await httpContext.Response.WriteAsJsonAsync(model, ExceptionHelper.Options, cancellationToken);
        return true;
    }
}