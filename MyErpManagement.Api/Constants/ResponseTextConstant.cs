namespace MyErpManagement.Api.Constants
{
    public class ResponseTextConstant
    {
        public static class BadRequest
        {
            public const string RequiredQueryAccessToken = "沒有Query access_token參數";
            public const string InvalidAccount = "無效的帳號";
            public const string InvalidPassword = "無效的密碼";
            public const string FailToSaveDb = "資料儲存失敗";
            public const string InvalidDto = "參數驗證失敗";
            public const string FailToUpdateCustomer = "客戶更新失敗";
            public const string FailToCreateCustomer = "客戶新增失敗";
            public const string FailToDeleteCustomer = "客戶移除失敗";
        }
        public static class UnAuthorized
        {
            public const string InvalidToken = "權杖無效,請重新登入";
            public const string TokenNotFromServer = "權杖不是由此伺服器提供,請重新登入";
        }

        public static class Forbidden
        {
            public const string NoPermission = "沒有權限操作此功能";
        }

        public static class NotFound
        {
            public const string User = "使用者不存在";
            public const string Customer = "客戶不存在";
        }

        public static class Conflict
        {
            public const string DbEntityUniqueField = "資料重複";
        }
    }
}
