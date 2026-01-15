using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.Entities;

namespace MyErpManagement.Core.Modules.JwtModule.IRepositories
{
    public interface IJwtRecordRepository : IRepository<JwtRecord>
    {
        //Task AddAsync(JwtRecord record);
        Task<JwtRecord?> GetByIdAsync(Guid id);
        // 用於強制登出某個使用者的所有裝置
        Task RevokeAllByUserIdAsync(Guid userId);
        // 定期清理已過期 Token 用
        Task RemoveExpiredAsync();
        Task<JwtRecord?> GetJwtRecordByAccessToken(string? token);
    }
}
