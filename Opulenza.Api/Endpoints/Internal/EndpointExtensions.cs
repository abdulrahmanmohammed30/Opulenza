using System.Reflection;

namespace Opulenza.Api.Endpoints.Internal;

public static class EndpointExtensions
{
    // used to scan everything in a given assembly and find every class that implements IEndpoints interface 
    // and dynamically call the AddServices and the DefineEndpoints method  
    public static void AddEndpoints<TMarker>(this IServiceCollection services, IConfiguration configuration)
    {
        AddEndpoints(services, typeof(TMarker), configuration);
    }

    private static void AddEndpoints(this IServiceCollection services, Type typeMarker, IConfiguration configuration)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.AddServices), BindingFlags.Static | BindingFlags.Public)!
               .Invoke(null, [services, configuration]);
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
    {
        var endpointTypes = typeMarker.Assembly.DefinedTypes
            .Where(t => t is { IsAbstract: false, IsInterface: false } && typeof(IEndpoints).IsAssignableFrom(t));
        return endpointTypes;
    }

    public static void UseEndpoints<TMarker>(this IApplicationBuilder app)
    {
        UseEndpoints(app, typeof(TMarker));
    }

    private static void UseEndpoints(this IApplicationBuilder app, Type typeMarker)
    {
        var endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);
        
        foreach (var endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.DefineEndpoints), BindingFlags.Static | BindingFlags.Public)!
                .Invoke(null, [app]);
        }
    }
}