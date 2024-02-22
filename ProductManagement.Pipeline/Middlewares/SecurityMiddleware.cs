using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Pipeline.Middlewares
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private const int MaxAge = 365;
        private IEnumerable<string> ExcludedHosts { get; } = new List<string>
        {
            "localhost",
            "127.0.0.1",
            "[::1]"
        };

        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (ExcludedHosts.Any(excludedHost =>
                    string.Equals(context.Request.Host.Host, excludedHost, StringComparison.OrdinalIgnoreCase)))
                return _next(context);

            //configure security headers here

            return _next(context);
        }
    }
}
