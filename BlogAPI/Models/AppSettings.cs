namespace BlogAPI.Models;

public class AppSettings:IAppSettings
{
    public string Token { get; set; } = string.Empty;
}