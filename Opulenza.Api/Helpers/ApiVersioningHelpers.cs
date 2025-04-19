using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace Opulenza.Api.Helpers;

public static class ApiVersioningHelpers
{
    public static ApiVersionSet ApiVersionSetV2 { get; private set; }

    public static void SetupApiVersionSet(IEndpointRouteBuilder app)
    {
        ApiVersionSetV2 = app
            .NewApiVersionSet()
            .HasApiVersion(2.0)
            .Build();
    }
}
