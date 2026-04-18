using MyAppLib.Helpers;

namespace MyAppLib.AppStates;

public class AppConfigState : IJsonConvertible
{
    public string Theme { get; set; } = "Default";
    public double FontSize { get; set; } = 12.0;
    public string LastUserId { get; set; } = string.Empty;
    public string LastUserPassword { get; set; } = string.Empty; // Encryption required
}