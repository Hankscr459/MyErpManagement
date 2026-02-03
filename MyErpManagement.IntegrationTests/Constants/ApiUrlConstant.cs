namespace MyErpManagement.IntegrationTests.Constants
{
    public static class ApiUrlConstant
    {
        public static class Auth
        {
            public const string Login = "/api/auth/login";
            public const string Register = "/api/auth/register";
            public const string VerifyEmail = "/api/auth/verify-email/register";
        }
        public static class Product
        {
            public const string ProductCRUD = "/api/product";
        }

        public static class Customer
        {
            public const string CustomerCRUD = "/api/customer";
        }
    }
}
