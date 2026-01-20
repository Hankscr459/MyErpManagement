namespace MyErpManagement.Core.Modules.EmailModule.IServices
{
    public interface IEmailService
    {
        Task<bool> SendRegisterCode(string email, string verificationRegistCode);
    }
}
