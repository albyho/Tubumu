using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Application.Services
{
    public interface IDataVersionService
    {
        Task<bool> SetAsync(int typeId);

        Task<DataVersion> GetAsync(int typeId);

        Task<IEnumerable<DataVersion>> GetAllAsync();

        Task CleanupAsync(int typeId);
    }
}
