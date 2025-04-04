namespace Opulenza.Contracts.Auth;

public class ExternalLoginResponse: LoginResponse
{
    public required string ReturnUrl { get; set; }
}