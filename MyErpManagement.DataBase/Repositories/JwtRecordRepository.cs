using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Modules.JwtModule.Entities;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class JwtRecordRepository : Repository<JwtRecord>, IJwtRecordRepository
    {
        private ApplicationDbContext _db;
        public JwtRecordRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<JwtRecord?> GetByIdAsync(Guid id)
        {
            return await _db.JwtRecords.FindAsync(id);
        }

        public async Task RevokeAllByUserIdAsync(Guid userId)
        {
            var records = await _db.JwtRecords
                .Where(r => r.UserId == userId && !r.IsRevoked)
                .ToListAsync();

            foreach (var record in records)
            {
                record.IsRevoked = true;
            }
        }

        public async Task RemoveExpiredAsync()
        {
            var expired = _db.JwtRecords.Where(r => r.ExpiresAt < DateTime.UtcNow);
            _db.JwtRecords.RemoveRange(expired);
            await _db.SaveChangesAsync();
        }

        public async Task<JwtRecord?> GetJwtRecordByAccessToken(string? token)
        {
            return await _db.JwtRecords
                .SingleOrDefaultAsync(x => x.TokenValue == token);
        }
    }
}
