namespace HomeTownPickEm.Security;

public class OriginOptions
{
    public string[] AllowedOrigins { get; set; }

    public void ValidateOrigin(string origin)
    {
        if (string.IsNullOrEmpty(origin))
        {
            throw new UnauthorizedAccessException("Origin header is required");
        }

        if (!AllowedOrigins.Contains(origin))
        {
            throw new UnauthorizedAccessException($"Origin '{origin}' is not allowed");
        }
    }
}
