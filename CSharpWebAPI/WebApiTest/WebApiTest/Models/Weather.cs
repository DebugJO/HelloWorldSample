using System;
using System.Formats.Asn1;

namespace WebApiTest.Models;

public class Weather
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; } = string.Empty;

    public string TestMessage { get; set; } = string.Empty;
}

public class RequestWeather
{
    public string TestMessage { get; set; } = string.Empty;
}