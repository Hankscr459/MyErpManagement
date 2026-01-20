namespace MyErpManagement.Core.Modules.CacheModule.IServices
{
    public interface ICachService
    {
        Task<int> SaveRegisterCodeAsync(string email, string code);
        Task<string?> GetRegistCodeAsync(string email);
    }
}
