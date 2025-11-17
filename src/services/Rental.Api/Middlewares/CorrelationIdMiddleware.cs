using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;
using System;

namespace Rental.Api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-ID";

        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Get correlation ID from request or generate new one
            var correlationId = context.Request.Headers.ContainsKey(HeaderName)
                ? context.Request.Headers[HeaderName].ToString()
                : Guid.NewGuid().ToString();

            context.Items[HeaderName] = correlationId;
            context.Response.Headers[HeaderName] = correlationId;

            // Push to Serilog
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
