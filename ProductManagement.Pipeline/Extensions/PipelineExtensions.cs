using Microsoft.AspNetCore.Builder;
using ProductManagement.Pipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Pipeline.Extensions
{
    public static class PipelineExtensions
    {
        public static IApplicationBuilder ConfigureApplicationPipeline(this IApplicationBuilder builder)
        {
            builder.UseGlobalExceptionHandler().UseSecurityMiddleware();
            return builder;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityMiddleware>();
        }
    }
}
