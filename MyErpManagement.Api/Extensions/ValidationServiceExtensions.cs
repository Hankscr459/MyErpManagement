using FluentValidation;
using FluentValidation.AspNetCore;
using MyErpManagement.Core.Validators.Interfaces;

namespace MyErpManagement.Api.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddCoreValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ICoreAssemblyMarker>();
            services.AddFluentValidationAutoValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
            });
            return services;
        }
    }
}
