using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Helpers;
using WebApiTest.Models;
using WebApiTest.Repository;

namespace WebApiTest.Services;

public class TestService : ITestService
{
    public async Task<DbResult<Weather>> GetWeather(string message)
    {
        try
        {
            string[] Summaries =
            [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            ];

            await Task.Delay(50);

            List<Weather> weatherList = Enumerable.Range(1, 5).Select(index => new Weather
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                TestMessage = message + index
            })
                .ToList();

            Weather? weather = weatherList.FirstOrDefault();

            return new DbResult<Weather>
            {
                Kind = weather != null ? EResultKind.OK : EResultKind.EMPTY,
                Result = weather ?? new Weather()
            };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"TestService(GetWeatherForecast) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<Weather> { Kind = EResultKind.ERROR, Error = ex.Message };
        }
    }

    public async Task<DbResult<List<Weather>>> GetWeatherForecast(string message)
    {
        try
        {
            string[] Summaries =
            [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            ];

            await Task.Delay(50);

            List<Weather> weatherList = Enumerable.Range(1, 5).Select(index => new Weather
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                TestMessage = message + index
            })
                .ToList();

            return new DbResult<List<Weather>>
            {
                Kind = weatherList.Count > 0 ? EResultKind.OK : EResultKind.EMPTY,
                Result = weatherList
            };
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"TestService(GetWeatherForecast) : ERROR : {ex.Message.ToSingleLine()}");
            return new DbResult<List<Weather>> { Kind = EResultKind.ERROR, Error = ex.Message };
        }
    }

    public async Task<DbResult<MenuConfig>> GetMenuConfig(string confName)
    {
        const string SQL = """
                           
                              select 
                                  t.CONF_NAME, 
                                  t.CONF_VERSION, 
                                  t.CONF_TYPE,   
                                  t.CONF_TIME, 
                                  t.CONF_DATA 
                              from tbMenuConfig t
                              where t.CONF_NAME = @CONF_NAME collate nocase
                                   
                           """;

        DynamicParameters param = new();
        param.Add("@CONF_NAME", confName, DbType.String, ParameterDirection.Input, confName.Length);
        return await DapperORM.ReturnSingleAsyncLite<MenuConfig>(SQL, new DynamicParameters());
    }

    public async Task<DbResult<List<MenuConfig>>> GetMenuConfigList(string confName)
    {
        const string SQL_W = """
                             
                                select 
                                    t.CONF_NAME, 
                                    t.CONF_VERSION, 
                                    t.CONF_TYPE,   
                                    t.CONF_TIME, 
                                    t.CONF_DATA 
                                from tbMenuConfig t
                                where t.CONF_NAME = @CONF_NAME collate nocase
                                     
                             """;

        const string SQL_A = """
                             
                                select 
                                    t.CONF_NAME, 
                                    t.CONF_VERSION, 
                                    t.CONF_TYPE,   
                                    t.CONF_TIME, 
                                    t.CONF_DATA 
                                from tbMenuConfig t
                                      
                             """;

        DynamicParameters param = new();

        if (string.IsNullOrWhiteSpace(confName))
        {
            return await DapperORM.ReturnListAsyncLite<MenuConfig>(SQL_A, param);
        }

        param.Add("@CONF_NAME", confName, DbType.String, ParameterDirection.Input, confName.Length);
        return await DapperORM.ReturnListAsyncLite<MenuConfig>(SQL_W, param);
    }
}