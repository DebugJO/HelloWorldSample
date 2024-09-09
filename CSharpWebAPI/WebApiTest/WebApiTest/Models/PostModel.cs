using System;
using WebApiTest.Helpers;
using WebApiTest.Repository;

namespace WebApiTest.Models;

public class PostModel<T> where T : class, new()
{
    public PostHeader Header { get; set; } = new();
    public T Body { get; set; } = new();
}

public class PostHeader
{
    public string RequestKey { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string RequestCommand { get; set; } = string.Empty;
}

public class ReturnModel<T> where T : class, new()
{
    public ReturnHeader Header { get; set; } = new();
    public T Body { get; set; } = new();

    public static ReturnModel<T> ReturnOkModel(T obj, EResultKind eResultKind)
    {
        try
        {
            ReturnModel<T> returnModel = new()
            {
                Header = new ReturnHeader
                {
                    ResultKind = eResultKind.ToString(),
                    ResultCode = "200",
                    ResultMessage = eResultKind == EResultKind.OK ? "정상처리" : "정상처리(결과없음)"
                },
                Body = obj
            };

            return returnModel;
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("ReturnWarnModel ERROR : " + ex.Message);
            return new ReturnModel<T>();
        }
    }
}

public class ReturnHeader
{
    public string ResultKind { get; set; } = string.Empty;
    public string ResultCode { get; set; } = string.Empty;
    public string ResultMessage { get; set; } = string.Empty;
}