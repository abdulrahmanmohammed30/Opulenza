namespace Opulenza.Application.Features.Authentication.Queries.Login;

public class TokenResult
{
  public string Token { get; set; } = null!;

  public DateTime Expiration {get; set;}
  
  public string RefreshToken { get; set; } = null!;
}