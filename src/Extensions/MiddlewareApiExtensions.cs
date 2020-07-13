using Microsoft.AspNetCore.Builder;
using Solidcube.AspNetCore.ProxyHelper.Models;

namespace Solidcube.AspNetCore.ProxyHelper.Extensions
{
    public static class MiddlewareApiExtensions
    {
        /// <summary>
        /// The default heder key name.
        /// </summary>
        public static readonly string XForwardedHeaderPath = "X-Forwarded-Path";

        /// <summary>
        /// Uses the forwarded header path.
        /// Read the path from the header key "X-Forwarded-Path".
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>Tha application.</returns>
        public static IApplicationBuilder UseForwardedHeaderPath(this IApplicationBuilder app)
        {
            ProxyPathOptions o = new ProxyPathOptions { XForwardedHeaderPath = XForwardedHeaderPath };
            return app.UseMiddleware<ProxyPathMiddleware>(o);
        }

        /// <summary>
        /// Uses the forwarded header path. 
        /// Read the path from the hader key specified by the options.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="options">The options.</param>
        /// <returns>The application.</returns>
        /// <exception cref="System.ArgumentNullException">options</exception>
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