namespace Opulenza.Api;

public static class RedisKeys
{
    public static string GetCartKey(string key) => $"cart#{key}";
}