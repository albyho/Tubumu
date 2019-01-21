using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Tubumu.Modules.Framework.SignalR
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
