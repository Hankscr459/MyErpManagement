using System.ComponentModel.DataAnnotations;

namespace MyErpManagement.Core.Dtos.Auth.Request
{
    public class VerifyEmailRequestDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
    }
}
