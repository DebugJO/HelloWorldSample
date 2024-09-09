using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Helpers;
using WebApiTest.Models;
using WebApiTest.Repository;
using WebApiTest.Services;

namespace WebApiTest.Controllers;

[ApiController]
public class TestController(ITestService testService) : ControllerBase
{
    [HttpPost("WeatherForecastList")]
    public async Task<IActionResult> WeatherForecastList([FromBody] PostModel<RequestWeather> inputModel)
    {
        try
        {
#if DEBUG
            LogHelper.Logger.Info($"PostInfo : {await HttpHelper.GetRequestInfo(HttpContext)} : {await HttpHelper.GetRequestIpAddress(HttpContext)}");
#endif
            DbResult<List<Weather>> result = await testService.GetWeatherForecast(inputModel.Body.TestMessage);

            return result.Kind == EResultKind.ERROR ?
                Ok(ErrorModel.ReturnErrorModel(result.Error)) :
                Ok(ReturnModel<List<Weather>>.ReturnOkModel(result.Result, result.Kind));
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("TestController(WeatherForecastList) ERROR : " + ex.Message);
            return Ok(ErrorModel.ReturnErrorModel(ex.Message));
        }
    }

    [HttpPost("WeatherForecast")]
    public async Task<IActionResult> WeatherForecast([FromBody] PostModel<RequestWeather> inputModel)
    {
        try
        {
#if DEBUG
            LogHelper.Logger.Info($"PostInfo : {await HttpHelper.GetRequestInfo(HttpContext)} : {await HttpHelper.GetRequestIpAddress(HttpContext)}");
#endif
            DbResult<Weather> result = await testService.GetWeather(inputModel.Body.TestMessage);

            return result.Kind == EResultKind.ERROR ?
                Ok(ErrorModel.ReturnErrorModel(result.Error)) :
                Ok(ReturnModel<Weather>.ReturnOkModel(result.Result, result.Kind));
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("TestController(WeatherForecast) ERROR : " + ex.Message);
            return Ok(ErrorModel.ReturnErrorModel(ex.Message));
        }
    }

    [HttpPost("MenuConfigList")]
    public async Task<IActionResult> MenuConfigList([FromBody] PostModel<RequestMenuConfig> inputModel)
    {
        try
        {
#if DEBUG
            LogHelper.Logger.Info($"PostInfo : {await HttpHelper.GetRequestInfo(HttpContext)} : {await HttpHelper.GetRequestIpAddress(HttpContext)}");
#endif
            DbResult<List<MenuConfig>> result = await testService.GetMenuConfigList(inputModel.Body.CONF_NAME);

            return result.Kind == EResultKind.ERROR ?
                Ok(ErrorModel.ReturnErrorModel(result.Error)) :
                Ok(ReturnModel<List<MenuConfig>>.ReturnOkModel(result.Result, result.Kind));
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("TestController(MenuConfigList) ERROR : " + ex.Message);
            return Ok(ErrorModel.ReturnErrorModel(ex.Message));
        }
    }

    [HttpPost("MenuConfig")]
    public async Task<IActionResult> MenuConfig([FromBody] PostModel<RequestMenuConfig> inputModel)
    {
        try
        {
            //int a = 1;
            //int b = 0;
            //int c = a / b;

            //throw new NotImplementedException("NotImplementedException");

            //미사용 무한루프
            //await Task.Delay(50);
            //DbResult<MenuConfig> test = await WebApiClient.PostApiTest<MenuConfig, RequestMenuConfig>("http://localhost:8088/MenuConfig", inputModel);
            //LogHelper.Logger.Debug($"무한루프 주의 : {JsonHelper.ModelToJson(test).JsonPretty()}");
#if DEBUG
            LogHelper.Logger.Info($"PostInfo : {await HttpHelper.GetRequestInfo(HttpContext)} : {await HttpHelper.GetRequestIpAddress(HttpContext)}");
#endif
            DbResult<MenuConfig> result = await testService.GetMenuConfig(inputModel.Body.CONF_NAME);

            return result.Kind == EResultKind.ERROR ?
                Ok(ErrorModel.ReturnErrorModel(result.Error)) :
                Ok(ReturnModel<MenuConfig>.ReturnOkModel(result.Result, result.Kind));
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error("TestController(MenuConfig) ERROR : " + ex.Message);
            return Ok(ErrorModel.ReturnErrorModel(ex.Message));
        }
    }
}