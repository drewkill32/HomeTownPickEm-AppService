namespace HomeTownPickEm.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; } // Map the token with jwtId
    public DateTimeOffset AddedDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; } // Refresh token is long lived it could last for months.

    public string UserId { get; set; } // Linked to the AspNet Identity User Id
    public ApplicationUser User { get; set; }
    public string IpAddress { get; set; }
}