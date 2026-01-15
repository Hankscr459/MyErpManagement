using System.Security.Claims;

namespace MyErpManagement.Api.Helpers
{
    public static class ClaimsPrincipleHelper
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue("userId"
                ?? throw new Exception("Cannot get username from token"));

            return new Guid(userId);
        }
    }
}
