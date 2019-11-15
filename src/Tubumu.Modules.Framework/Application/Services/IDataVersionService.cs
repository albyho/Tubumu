using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Application.Services
{
    public interface IDataVersionService
    {
        Task<bool> SetGlobalAsync(int typeId);

        Task<bool> SetAsync(string keyPrefix, int typeId);

        Task<DataVersion> GetGlobalAsync(int typeId);

        Task<DataVersion> GetAsync(string keyPrefix, int typeId);

        Task<IEnumerable<DataVersion>> GetAllAsync();

        Task<IEnumerable<DataVersion>> GetGlobalAllAsync();

        Task<IEnumerable<DataVersion>> GetAllAsync(string keyPrefix);

        Task CleanupGlobalAsync(int typeId);

        Task CleanupAsync(string keyPrefix, int typeId);
    }
}
