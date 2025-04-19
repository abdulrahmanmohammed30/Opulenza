namespace Opulenza.Application.Authentication;

public static class AuthConstants
{
    public const string AdminUserPolicyName = "Admin";
    public const string AdminUserClaimName = "admin";
    
    public const string ApiKeySectionName = "Authentication:ApiKey";
    public const string ApiKeyHeaderName = "X-Api-Key";
    public const string ApiKeyPolicyName = "ApiKey";
}