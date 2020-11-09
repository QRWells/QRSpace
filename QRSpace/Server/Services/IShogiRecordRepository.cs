using QRSpace.Server.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public interface IShogiRecordRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<ShogiRecord> GetShogiRecordByIdAsync(ulong recordId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<ShogiRecord>> GetShogiRecordsByUserAsync(ApplicationUser user);

        void AddShogiRecord(ShogiRecord record);

        /// <summary>
        ///
        /// </summary>
        /// <param name="record"></param>
        void UpdateShogiRecord(ShogiRecord record);

        /// <summary>
        ///
        /// </summary>
        /// <param name="record"></param>
        void DeleteShogiRecord(ShogiRecord record);
    }
}