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

    /// <summary>
    /// Http Module to ensure that values set in the audit logging context do not mix between requests.
    /// 
    /// Hooks into the Application_BeginRequest to initialize the context with default values.
    /// * Sets correlation id to a new Guid value.
    /// * Sets ssid to an empty Guid value (all zeroes).
    /// 
    /// Hooks into the Application_EndRequest to clear the context.
    /// * Does exactly the same as the begin request -hook.
    /// </summary>
    public class AuditLoggerHttpModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += new EventHandler(Application_BeginRequest);
            application.EndRequest += new EventHandler(Application_EndRequest);
        }
        
        private void Application_BeginRequest(object source, EventArgs e)
        {
            AuditLoggerContext.Initialize();
        }

        private void Application_EndRequest(object source, EventArgs e)
        {
            AuditLoggerContext.Clear();
        }

        public void Dispose() { }
    }
    
#elif NETSTANDARD2_0_OR_GREATER

    /// <summary>
    /// Middleware to ensure that values set in the audit logging context do not mix between requests.
    /// 
    /// Upon invocation
    /// * Sets correlation id to a new Guid value.
    /// * Sets ssid to an empty Guid value (all zeroes).
    /// 
    /// Afterwards
    /// * Does exactly the same as the begin request -hook.
    /// </summary>
    public class AuditLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            AuditLoggerContext.Initialize();
            try
            {
                await _next(context);
            }
            finally
            {
                AuditLoggerContext.Clear();
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
