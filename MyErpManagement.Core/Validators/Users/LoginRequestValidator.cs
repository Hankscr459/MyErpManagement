using FluentValidation;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Validators.Constants;

namespace MyErpManagement.Core.Validators.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Account)
                .NotEmpty().WithMessage(VaildatorConstant.Required)
                .MinimumLength(4).WithMessage(VaildatorConstant.MinLengthFormat);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(VaildatorConstant.Required)
                .MinimumLength(4).WithMessage(VaildatorConstant.MinLengthFormat);
        }
    }
}
