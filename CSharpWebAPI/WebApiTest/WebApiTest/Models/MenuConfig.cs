using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiTest.Models;

public class MenuConfig
{
    [JsonPropertyName("ConfigName")]
    public string CONF_NAME { get; set; } = string.Empty;

    [JsonPropertyName("ConfigVersion")]
    public string CONF_VERSION { get; set; } = string.Empty;

    [JsonPropertyName("ConfigType")]
    public string CONF_TYPE { get; set; } = string.Empty;

    [JsonPropertyName("ConfigTime")]
    public string CONF_TIME { get; set; } = string.Empty;

    [JsonPropertyName("ConfigData")]
    public string CONF_DATA { get; set; } = string.Empty;
}

public class RequestMenuConfig
{
    [JsonPropertyName("ConfigName")]
    [Required, StringLength(20, MinimumLength = 3)]
    public string CONF_NAME { get; set; } = string.Empty;
}
