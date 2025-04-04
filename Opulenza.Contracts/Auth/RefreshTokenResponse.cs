namespace Opulenza.Contracts.Auth;

public class RefreshTokenResponse
{
    public string Token { get; set; } = null!;

    public DateTime Expiration {get; set;}
  
    public string RefreshToken { get; set; } = null!;
}