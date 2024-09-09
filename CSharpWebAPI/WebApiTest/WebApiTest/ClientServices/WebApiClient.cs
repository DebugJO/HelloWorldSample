using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiTest.Helpers;
using WebApiTest.Models;
using WebApiTest.Repository;

namespace WebApiTest.ClientServices;

public static class WebApiClient
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null,
    };

    private static readonly JsonSerializerOptions OptionsEscapes = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = null
    };

    public static async Task<DbResult<T>> GetApiTest<T>(string url, Dictionary<string, string?> param, bool isEscapes = false) where T : class, new()
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("GP-AUTH-ID", "11111");
            //client.DefaultRequestHeaders.Add("GP-AUTH-TOKEN", "22222");

            //Dictionary<string, string?> param1 = new()
            //{
            //    ["posNo"] = "01",
            //    ["finalDt"] = DateTime.Now.ToString("yyyyMMddHHmmss")
            //};

            if (!url.IsURL())
            {
                return new DbResult<T> { Kind = EResultKind.ERROR, Error = "URL이 올바르지 않습니다" };
            }

            string query = QueryHelpers.AddQueryString($"{url}", param);

            using HttpResponseMessage response = await client.GetAsync(query);

            T? obj = await response.Content.ReadFromJsonAsync<T>(isEscapes ? OptionsEscapes : Options);

            //string json = JsonHelper.ModelToJson(a);
            //string status = json.GetJsonValue("status");

            if (response.IsSuccessStatusCode)
            {
                return new DbResult<T>
                {
                    Kind = obj != null ? EResultKind.OK : EResultKind.EMPTY,
                    Result = obj ?? new T()
                };
            }

            string message = response.StatusCode.GetHashCode() + " / " + response.ReasonPhrase;
            return new DbResult<T> { Kind = EResultKind.ERROR, Error = message };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"WebApiClient(GetApiTest) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<T> { Kind = EResultKind.ERROR, Error = ex.Message };
        }
    }

    public static async Task<DbResult<T>> PostApiTest<T, U>(string url, PostModel<U> requestModel, bool isEscapes = false) where T : class, new() where U : class, new()
    {
        try
        {
            using HttpClient client = new();
            //using HttpClientHandler clientHandler = new();
            //clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            //using HttpClient client = new(clientHandler);

            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("GP-AUTH-ID", "11111");
            //client.DefaultRequestHeaders.Add("GP-AUTH-TOKEN", "22222");

            if (!url.IsURL())
            {
                return new DbResult<T> { Kind = EResultKind.ERROR, Error = "URL이 올바르지 않습니다" };
            }


            if (!JsonHelper.ModelToJson(requestModel).IsJsonValid())
            {
                return new DbResult<T> { Kind = EResultKind.ERROR, Error = "JSON 규격이 올바르지 않습니다" };
            }

            using HttpResponseMessage response = await client.PostAsJsonAsync(url, requestModel, isEscapes ? OptionsEscapes : Options);

            T? obj = await response.Content.ReadFromJsonAsync<T>(Options);

            if (response.IsSuccessStatusCode)
            {
                return new DbResult<T>
                {
                    Kind = obj != null ? EResultKind.OK : EResultKind.EMPTY,
                    Result = obj ?? new T()
                };
            }

            string message = response.StatusCode.GetHashCode() + " / " + response.ReasonPhrase;
            return new DbResult<T> { Kind = EResultKind.ERROR, Error = message };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"WebApiClient(PostApiTest) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<T> { Kind = EResultKind.ERROR, Error = ex.Message };
        }
    }
}