using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Filters;

namespace MyErpManagement.Api.Attributes
{
    public class HasQueryTokenAttribute : TypeFilterAttribute
    {

        public HasQueryTokenAttribute() : base(typeof(HasQueryTokenFilter))
        {
        }
    }
}
