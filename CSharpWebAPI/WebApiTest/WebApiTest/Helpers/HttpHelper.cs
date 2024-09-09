using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTest.Helpers;

public static class HttpHelper
{
    private static readonly Dictionary<int, string> code = new()
    {
        [100] = "Continue",
        [101] = "Switching Protocols",
        [102] = "Processing",
        [103] = "Early Hints",

        [200] = "OK",
        [201] = "Created",
        [202] = "Accepted",
        [203] = "Non-Authoritative Information",
        [204] = "No Content",
        [205] = "Reset Content",
        [206] = "Partial Content",
        [207] = "Multi-Status",
        [208] = "Already Reported",
        [226] = "IM Used",

        [300] = "Multiple Choices",
        [301] = "Moved Permanently",
        [302] = "Found",
        [303] = "See Other",
        [304] = "Not Modified",
        [307] = "Temporary Redirect",
        [308] = "Permanent Redirect",

        [400] = "Bad Request",
        [401] = "Unauthorized",
        [402] = "Payment Required",
        [403] = "Forbidden",
        [404] = "Not Found",
        [405] = "Method Not Allowed",
        [406] = "Not Acceptable",
        [407] = "Proxy Authentication Required",
        [408] = "Request Timeout",
        [409] = "Conflict",
        [410] = "Gone",
        [411] = "Length Required",
        [412] = "Precondition Failed",
        [413] = "Content Too Long",
        [414] = "URI Too Long",
        [415] = "Unsupported Media Type",
        [416] = "Range Not Satisfiable",
        [417] = "Exception Failed",
        [418] = "I'm a teapot",
        [421] = "Misdirected Request",
        [422] = "Unprocessable Content",
        [423] = "Locked",
        [424] = "Failed Dependency",
        [425] = "Too Early",
        [426] = "Upgrade Required",
        [428] = "Precondition Required",
        [429] = "Too Many Requests",
        [431] = "Request Header Fields Too Large",
        [451] = "Unavailable For legal Reasons",

        [500] = "Internal Server Error",
        [501] = "Not Implemented",
        [502] = "Bad Gateway",
        [503] = "Service Unavailable",
        [504] = "Gateway Timeout",
        [505] = "HTTP Version Not Supported",
        [506] = "Variant Also Negotiates",
        [507] = "Insufficient Storage",
        [508] = "Loop Detected",
        [510] = "Not Extended",
        [511] = "Network Authentication Required",
        [520] = "Unknown Error",
        [598] = "Network Read Timeout",
        [599] = "Network Connection Timeout"
    };

    // https://developer.mozilla.org/ko/docs/Web/HTTP/Status

    //static HttpHelper()
    //{
    //    code.Add(100, "Continue");
    //    code.Add(101, "Switching Protocols");
    //    code.Add(102, "Processing");
    //    code.Add(103, "Early Hints");

    //    code.Add(200, "OK");
    //    code.Add(201, "Created");
    //    code.Add(202, "Accepted");
    //    code.Add(203, "Non-Authoritative Information");
    //    code.Add(204, "No Content");
    //    code.Add(205, "Reset Content");
    //    code.Add(206, "Partial Content");
    //    code.Add(207, "Multi-Status");
    //    code.Add(208, "Already Reported");
    //    code.Add(226, "IM Used");

    //    code.Add(300, "Multiple Choices");
    //    code.Add(301, "Moved Permanently");
    //    code.Add(302, "Found");
    //    code.Add(303, "See Other");
    //    code.Add(304, "Not Modified");
    //    code.Add(307, "Temporary Redirect");
    //    code.Add(308, "Permanent Redirect");

    //    code.Add(400, "Bad Request");
    //    code.Add(401, "Unauthorized");
    //    code.Add(402, "Payment Required");
    //    code.Add(403, "Forbidden");
    //    code.Add(404, "Not Found");
    //    code.Add(405, "Method Not Allowed");
    //    code.Add(406, "Not Acceptable");
    //    code.Add(407, "Proxy Authentication Required");
    //    code.Add(408, "Request Timeout");
    //    code.Add(409, "Conflict");
    //    code.Add(410, "Gone");
    //    code.Add(411, "Length Required");
    //    code.Add(412, "Precondition Failed");
    //    code.Add(413, "Content Too Long");
    //    code.Add(414, "URI Too Long");
    //    code.Add(415, "Unsupported Media Type");
    //    code.Add(416, "Range Not Satisfiable");
    //    code.Add(417, "Exception Failed");
    //    code.Add(418, "I'm a teapot");
    //    code.Add(421, "Misdirected Request");
    //    code.Add(422, "Unprocessable Content");
    //    code.Add(423, "Locked");
    //    code.Add(424, "Failed Dependency");
    //    code.Add(425, "Too Early");
    //    code.Add(426, "Upgrade Required");
    //    code.Add(428, "Precondition Required");
    //    code.Add(429, "Too Many Requests");
    //    code.Add(431, "Request Header Fields Too Large");
    //    code.Add(451, "Unavailable For legal Reasons");

    //    code.Add(500, "Internal Server Error");
    //    code.Add(501, "Not Implemented");
    //    code.Add(502, "Bad Gateway");
    //    code.Add(503, "Service Unavailable");
    //    code.Add(504, "Gateway Timeout");
    //    code.Add(505, "HTTP Version Not Supported");
    //    code.Add(506, "Variant Also Negotiates");
    //    code.Add(507, "Insufficient Storage");
    //    code.Add(508, "Loop Detected");
    //    code.Add(510, "Not Extended");
    //    code.Add(511, "Network Authentication Required");
    //    code.Add(520, "Unknown Error");
    //    code.Add(598, "Network Read Timeout");
    //    code.Add(599, "Network Connection Timeout");
    //}

    public static string GetCodeMessage(int statusCode)
    {
        return code.GetValueOrDefault(statusCode, "ETC or UNKNOWN");
    }

    public static async Task<string> GetRequestInfo(HttpContext context)
    {
        try
        {
            if (!context.Request.Body.CanRead)
            {
                return "Request.Body(1) Stream was not readable(Length 0)";
            }

            using StreamReader reader = new(context.Request.Body, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string result = await reader.ReadToEndAsync().ConfigureAwait(false);
            return result.ToSingleLine();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"GetRequestInfo ERROR : {ex.Message}");
            return ex.Message;
        }
    }

    public static string GetRequestValidInfo(HttpContext context)
    {
        try
        {
            if (!context.Request.Body.CanRead)
            {
                return "Request.Body(2) Stream was not readable(Length 0)";
            }

            using StreamReader reader = new(context.Request.Body, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string result = reader.ReadToEnd();
            return result.ToSingleLine();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"GetRequestValidInfo ERROR : {ex.Message}");
            return ex.Message;
        }
    }

    public static async Task<string> GetRequestIpAddress(HttpContext context)
    {
        return await Task.Run(() =>
        {
            try
            {
                return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? context.Connection.RemoteIpAddress?.ToString() ?? "Server IP Address Could Not Be Found";
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"GetRequestInfo ERROR : {ex.Message}");
                return ex.Message;
            }
        });
    }

    public static string GetRequestValidIpAddress(HttpContext context)
    {
        try
        {
            return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? context.Connection.RemoteIpAddress?.ToString() ?? "Server IP Address Could Not Be Found";
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"GetRequestInfo ERROR : {ex.Message}");
            return ex.Message;
        }
    }

    public static async Task SetPostLog(string methodName, string lineNumber, HttpContext context)
    {
        try
        {
            string inputJson = await GetRequestInfo(context);
            string inputIpAddress = await GetRequestIpAddress(context);
            LogHelper.Logger.Info($"SetPostLog : {methodName}({lineNumber}) : {inputJson} : {inputIpAddress}");
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"SetPostLog ERROR : {ex.Message}");
        }

    }

    public static int LineNumber([CallerLineNumber] int lineNumber = 0)
    {
        return lineNumber;
    }

    public static string MethodName([CallerMemberName] string caller = "")
    {
        return caller;
    }
}