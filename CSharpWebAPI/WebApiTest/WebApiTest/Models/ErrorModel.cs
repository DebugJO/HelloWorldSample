using System;
using WebApiTest.Helpers;
using WebApiTest.Repository;

namespace WebApiTest.Models;

public class ErrorModel
{
    public ErrorHeader Header { get; set; } = new();
    public ErrorBody Body { get; set; } = new();

    public static ErrorModel ReturnErrorModel(string errorMessage)
    {
        try
        {
            ErrorModel errorModel = new()
            {
                Header = new ErrorHeader
                {
                    ResultCode = "500",
                    ResultMessage = errorMessage
                },
                Body = new ErrorBody
                {
                    ResultMessageDetail = "Internal Server Error"
                }
            };

            return errorModel;
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("ReturnErrorModel ERROR : " + ex.Message);
            return new ErrorModel();
        }
    }
}

public class ErrorHeader
{
    public string ResultKind { get; set; } = EResultKind.ERROR.ToString();
    public string ResultCode { get; set; } = string.Empty;
    public string ResultMessage { get; set; } = string.Empty;
}

public class ErrorBody
{
    public string ResultMessageDetail { get; set; } = string.Empty;
}