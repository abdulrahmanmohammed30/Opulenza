namespace Opulenza.Application.Common.Utilities;

public interface IUrlGenerator
{
    string? GenerateUrl(string action, string controller, object? routeValues);
}