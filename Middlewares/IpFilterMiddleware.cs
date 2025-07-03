using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ExamenApi.Middlewares
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AllowedIp = "187.155.101.200";

        public IpFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            if (remoteIp != AllowedIp)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied from this IP address.");
                return;
            }
            await _next(context);
        }
    }
}
