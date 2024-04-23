using CathaybkHW.Middleware;

namespace CathaybkHW.Extensions;

public static class MiddlewareExtension
{
    public static void AddMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
