namespace MyErpManagement.Core.Modules.MessageBusModule.Models
{
    public class EmailMessage
    {
        public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}
