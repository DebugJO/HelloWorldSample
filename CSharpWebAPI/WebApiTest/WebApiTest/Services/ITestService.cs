using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Models;
using WebApiTest.Repository;

namespace WebApiTest.Services;

public interface ITestService
{
    Task<DbResult<Weather>> GetWeather(string message);
    Task<DbResult<List<Weather>>> GetWeatherForecast(string message);
    Task<DbResult<MenuConfig>> GetMenuConfig(string confName);
    Task<DbResult<List<MenuConfig>>> GetMenuConfigList(string confName);
}