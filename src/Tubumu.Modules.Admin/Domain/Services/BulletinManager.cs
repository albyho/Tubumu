using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Domain.Entities;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IBulletinManager
    /// </summary>
    public interface IBulletinManager
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <returns></returns>
        Task<XM.Bulletin> GetItemAsync();

        /// <summary>
        /// BulletinInput
        /// </summary>
        /// <param name="bulletinInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(BulletinInput bulletinInput, ModelStateDictionary modelState);
    }

    /// <summary>
    /// BulletinManager
    /// </summary>
    public class BulletinManager : IBulletinManager
    {
        private readonly TubumuContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public BulletinManager(TubumuContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <returns></returns>
        public async Task<XM.Bulletin> GetItemAsync()
        {
            var item = await _context.Bulletin.OrderByDescending(m => m.BulletinId).AsNoTracking().FirstOrDefaultAsync();
            return item.MapTo<XM.Bulletin>();
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="bulletinInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(BulletinInput bulletinInput, ModelStateDictionary modelState)
        {
            var dbBulletin = await _context.Bulletin.OrderByDescending(m => m.BulletinId).FirstOrDefaultAsync();
            if (dbBulletin == null) return false;

            Mapper.Map(bulletinInput, dbBulletin);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
