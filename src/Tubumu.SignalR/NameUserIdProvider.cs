using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Tubumu.SignalR
{
    /// <summary>
    /// NameUserIdProvider
    /// </summary>
    public class NameUserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// GetUserId
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
