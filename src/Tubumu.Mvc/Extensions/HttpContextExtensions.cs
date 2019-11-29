using System.Net;
using Microsoft.AspNetCore.Http;

namespace Tubumu.Mvc.Extensions
{
    public static class HttpContextExtensions
    {
        public static IPAddress GetRealIp(this HttpContext context)
        {
            IPAddress ip;
            var headers = context.Request.Headers;
            if (!headers.ContainsKey("X-Forwarded-For") || !IPAddress.TryParse(headers["X-Forwarded-For"].ToString().Split(',')[0], out ip))
            {
                ip = context.Connection.RemoteIpAddress;
            }
            return ip;
        }
    }
}
