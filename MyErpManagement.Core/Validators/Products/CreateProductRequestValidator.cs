using FluentValidation;
using MyErpManagement.Core.Dtos.Products.Request;
using MyErpManagement.Core.Validators.Constants;

namespace MyErpManagement.Core.Validators.Products
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequestDto>
    {
        public Guid ProductCategoryId { get; set; }
        public CreateProductRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(VaildatorConstant.Required);

            RuleFor(p => p.Code)
                .NotEmpty().WithMessage(VaildatorConstant.Required);

            RuleFor(p => p.PurchasePrice)
                .NotEmpty().WithMessage(VaildatorConstant.Required);

            RuleFor(p => p.SalesPrice)
                .NotEmpty().WithMessage(VaildatorConstant.Required);

        }
    }
}
