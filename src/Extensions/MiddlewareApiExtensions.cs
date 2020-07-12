using Microsoft.AspNetCore.Builder;
using Solidcube.AspNetCore.ProxyHelper.Models;

namespace Solidcube.AspNetCore.ProxyHelper.Extensions
{
    public static class MiddlewareApiExtensions
    {
        public static readonly string XForwardedHeaderPath = "X-Forwarded-Path";

        public static IApplicationBuilder UseForwardedHeaderPath(this IApplicationBuilder app)
        {
            ProxyPathOptions o = new ProxyPathOptions { XForwardedHeaderPath = XForwardedHeaderPath };
            return app.UseMiddleware<ProxyPathMiddleware>(o);
        }

        public static IApplicationBuilder UseForwardedHeaderPath(this IApplicationBuilder app, ProxyPathOptions options)
        {
            if (options is null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<ProxyPathMiddleware>(options);
        }
    }
}