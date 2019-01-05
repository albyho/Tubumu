using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tubumu.Modules.Framework.Services
{
    public interface ISmsSender
    {
        Task<bool> SendAsync(string mobile, string content);
    }
}
