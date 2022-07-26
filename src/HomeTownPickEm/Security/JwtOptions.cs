namespace HomeTownPickEm.Security;

public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    public TimeSpan? Expiration { get; set; }
    public TimeSpan? RefreshExpiration { get; set; }
}