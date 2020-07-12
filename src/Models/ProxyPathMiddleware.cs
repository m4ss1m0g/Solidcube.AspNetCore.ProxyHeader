using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Solidcube.AspNetCore.ProxyHelper.Models
{
    /// <summary>
    /// Provide middleware for getting forwarded path from proxy server
    /// Used by OData path
    /// </summary>
    public class ProxyPathMiddleware
    {
        private readonly ProxyPathOptions _options;

        private readonly RequestDelegate _next;
         
        public ProxyPathMiddleware(RequestDelegate next, ProxyPathOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_options.XForwardedHeaderPath, out var value))
            {
                context.Request.PathBase = value.ToString();
            }

            await _next(context);
        }
    }
}