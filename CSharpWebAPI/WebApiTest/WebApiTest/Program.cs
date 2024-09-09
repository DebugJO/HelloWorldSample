using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiTest.Helpers;
using WebApiTest.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:8088", "https://*:8080");

typeof(Program).Assembly.GetTypes()
    .Where(type => type.IsClass)
    .Where(type => type.Name.EndsWith("Service")).ToList()
    .ForEach(vmType => builder.Services.AddSingleton(vmType.GetInterfaces().First(), vmType));

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressMapClientErrors = true;

    o.InvalidModelStateResponseFactory = x =>
    {

        string error = new ValidationProblemDetails(x.ModelState).Errors.Values.FirstOrDefault()?.FirstOrDefault() ?? "Bad Request(400) or Validation Error";

        LogHelper.Logger.Warn($"Program.Configure(InvalidModelStateResponseFactory) : Bad Request(400) or Validation Error : {error} : {HttpHelper.GetRequestValidIpAddress(x.HttpContext)}");

        return new BadRequestObjectResult(new ErrorModel
        {
            Header = new ErrorHeader
            {
                ResultCode = "400", //x.HttpContext.Response.StatusCode.ToString(),
                ResultMessage = "Bad Request(400) or Validation Error"
            },
            Body = new ErrorBody
            {
                ResultMessageDetail = $"{error} : {HttpHelper.GetRequestValidInfo(x.HttpContext)}"
            }
        });
    };
});

builder.Services.AddExceptionHandler<AppExceptionHandler>(); // app.UseExceptionHandler(appError => 아래 사용시 주석 처리해야함
builder.Services.AddExceptionHandler<AppNotImplementedExceptionHandler>(); // app.UseExceptionHandler(appError => 아래 사용시 주석 처리해야함

WebApplication app = builder.Build();

//app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();

    if (!context.Response.HasStarted)
    {
        await next(context);
    }

    if (!context.Response.StatusCode.ToString().StartsWith('2'))
    {
        await ReturnErrorModel(context);

        if (!context.Response.HasStarted)
        {
            await next(context);
        }

        LogHelper.Logger.Debug($"HTTP Response Status Code(Not 2XX) : {context.Response.StatusCode.ToString()}");

        if (!context.Response.HasStarted)
        {
            await next(context);
        }
    }
    else if (context.Response.StatusCode.ToString().StartsWith('2') && !context.Response.StatusCode.ToString().EndsWith('0'))
    {
        LogHelper.Logger.Debug($"HTTP Response Status Code(201~2XX) : {context.Response.StatusCode.ToString()}");

        if (!context.Response.HasStarted)
        {
            await next(context);
        }
    }
});

//app.UseExceptionHandler(appError =>
//{
//    appError.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        context.Response.ContentType = "application/json";

//        IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();

//        if (contextFeature is not null)
//        {
//            LogHelper.Logger.Fatal($"WebAPI Global Error : {contextFeature.Error.Message}");
//            await ReturnErrorModel(context, $"Internal WebAPI Server Error : {contextFeature.Error.Message}");
//        }
//    });
//});

app.UseExceptionHandler(_ => { });

app.Lifetime.ApplicationStarted.Register(() =>
{
    LogHelper.Logger.Info("***** WebAPI Server : Start *****");

});

app.Lifetime.ApplicationStopped.Register(() =>
{
    LogHelper.Logger.Info("***** WebAPI Server : Stop *****");
});

app.UseAuthorization();
app.MapControllers();
app.Run();

public static partial class Program
{
    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    private static async Task ReturnErrorModel(HttpContext context, string message = "")
    {
        int statusCode = context.Response.StatusCode;

        ErrorModel model = new()
        {
            Header = new ErrorHeader
            {
                ResultCode = statusCode.ToString(),
                ResultMessage = string.IsNullOrWhiteSpace(message) ? HttpHelper.GetCodeMessage(statusCode) : message
            },
            Body = new ErrorBody
            {
                ResultMessageDetail = $"{context.Request.Host.Value}{context.Request.Path} : {await HttpHelper.GetRequestInfo(context)}"
            }
        };

        await context.Response.WriteAsJsonAsync(model, serializerOptions);
    }
}