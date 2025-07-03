using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ExamenApi.Middlewares
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string[] AllowedIps = new[] { "127.0.0.1", "::1", "localhost", "187.155.101.200" };

        public IpFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            // Permitir acceso solo si la IP es local, localhost, o la IP permitida original
            if (remoteIp == null || (!AllowedIps.Contains(remoteIp) && !(remoteIp.StartsWith("192.168."))))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied from this IP address.");
                return;
            }
            await _next(context);
        }
    }
}
