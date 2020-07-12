using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Solidcube.AspNetCore.ProxyHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solidcube.AspNetCore.ProxyHelper.Tests.Models
{
    public class ProxyPathMiddlewareTest
    {
        [Theory]
        [InlineData("http://localhost/api/customers", "/commons")]
        [InlineData("http://localhost/api/customers/foo/bar", "/commons/shared")]
        [InlineData("http://localhost/bar", "/shared")]
        public async Task ProxyHeaderMiddlewareCorrectlySetupBasePath(string uri, string headerValue)
        {
            // Arrange
            Uri requestUri = new Uri(uri);
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseForwardedHeaderPath();

                            app.Run(o => o.Response.WriteAsync(o.Request.GetDisplayUrl()));
                        });
                })
                .StartAsync();

            // Act
            var request = new System.Net.Http.HttpRequestMessage();
            request.Headers.Add(MiddlewareApiExtensions.XForwardedHeaderPath, headerValue);
            request.RequestUri = requestUri;
            var response = await host.GetTestServer().CreateClient().SendAsync(request);

            // Assert
            Assert.Equal($"{requestUri.Scheme}://{requestUri.Host}{headerValue}{requestUri.AbsolutePath}", await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData("http://localhost/api/customers", "X-Custom", "/commons")]
        [InlineData("http://localhost/api/customers/foo/bar", "X-Custom", "/commons/shared")]
        [InlineData("http://localhost/bar", "X-Custom", "/shared")]
        public async Task ProxyHeaderMiddlewareWithCustomHeaderCorrectlySetupBasePath(string uri, string headerName, string headerValue)
        {
            // Arrange
            Uri requestUri = new Uri(uri);
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseForwardedHeaderPath(new ProxyHelper.Models.ProxyPathOptions { XForwardedHeaderPath = headerName });

                            app.Run(o => o.Response.WriteAsync(o.Request.GetDisplayUrl()));
                        });
                })
                .StartAsync();

            // Act
            var request = new System.Net.Http.HttpRequestMessage();
            request.Headers.Add(headerName, headerValue);
            request.RequestUri = requestUri;
            var response = await host.GetTestServer().CreateClient().SendAsync(request);

            // Assert
            Assert.Equal($"{requestUri.Scheme}://{requestUri.Host}{headerValue}{requestUri.AbsolutePath}", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ProxyHeaderMiddlewareDoNotSetupBAsePathWhenNoHeaderIsPassed()
        {
            // Arrange
            Uri requestUri = new Uri("http://localhost/api/customers");
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseForwardedHeaderPath();

                            app.Run(o => o.Response.WriteAsync(o.Request.GetDisplayUrl()));
                        });
                })
                .StartAsync();

            // Act
            var request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = requestUri
            };

            var response = await host.GetTestServer().CreateClient().SendAsync(request);

            // Assert
            Assert.Equal($"{requestUri.Scheme}://{requestUri.Host}{requestUri.AbsolutePath}", await response.Content.ReadAsStringAsync());
        }


        [Fact]
        public async Task ProxyHeaderMiddlewareThrowExceptionWhenPassingNullOptions()
        {
            // Arrange
            Uri requestUri = new Uri("http://localhost/api/customers");
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseForwardedHeaderPath(null);

                            app.Run(o => o.Response.WriteAsync(o.Request.GetDisplayUrl()));
                        });
                });


            await Assert.ThrowsAsync<ArgumentNullException>(() => hostBuilder.StartAsync());
        }
    }
}
