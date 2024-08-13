#if NET461_OR_GREATER
using System;
using System.Web;
#elif NETSTANDARD2_0_OR_GREATER
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
#endif

namespace Mercell.Cloudia.AuditLogging.Aspnet
{

#if NET461_OR_GREATER
    public class AuditLoggerHttpModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.EndRequest += new EventHandler(Application_EndRequest);
        }

        private void Application_EndRequest(object source, EventArgs e)
        {
            AuditLoggerContext.Reset();
        }

        public void Dispose() { }
    }
    
#elif NETSTANDARD2_0_OR_GREATER

    public class AuditLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                AuditLoggerContext.Reset();
            }
        }
    }

    public static class AuditLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditLoggerMiddleware>();
        }
    }

#endif

}
