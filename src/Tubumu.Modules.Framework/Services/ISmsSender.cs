using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tubumu.Modules.Framework.Services
{
    /// <summary>
    /// ISmsSender
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> SendAsync(string mobile, string content);
    }
}
