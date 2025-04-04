namespace Opulenza.Contracts.Auth;

public class ConfirmEmailRequest
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
}