namespace Opulenza.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next): IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}