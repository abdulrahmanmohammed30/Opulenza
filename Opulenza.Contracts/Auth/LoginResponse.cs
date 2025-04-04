namespace Opulenza.Contracts.Auth;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}