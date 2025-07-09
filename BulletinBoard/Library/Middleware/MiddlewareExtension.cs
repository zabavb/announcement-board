using Microsoft.AspNetCore.Builder;

namespace Library.Middleware;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) => 
        app.UseMiddleware<ExceptionMiddleware>();
}