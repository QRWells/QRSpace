using Microsoft.EntityFrameworkCore;
using QRSpace.Server.Data;
using QRSpace.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRSpace.Server.Services
{
    public class ShogiRecordRepository : IShogiRecordRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShogiRecordRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void AddShogiRecord(ShogiRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            _dbContext.ShogiRecords.Add(record);
        }

        public void DeleteShogiRecord(ShogiRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            _dbContext.ShogiRecords.Remove(record);
        }

        public async Task<ShogiRecord> GetShogiRecordByIdAsync(ulong recordId)
        {
            return await _dbContext.ShogiRecords.FirstOrDefaultAsync(r => r.RecordId == recordId);
        }

        public async Task<IEnumerable<ShogiRecord>> GetShogiRecordsByUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _dbContext.ShogiRecords.Where(r => r.User.Id == user.Id).OrderBy(r => r.RecordId).ToListAsync();
        }

        public void UpdateShogiRecord(ShogiRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            _dbContext.ShogiRecords.Update(record);
        }
    }
}