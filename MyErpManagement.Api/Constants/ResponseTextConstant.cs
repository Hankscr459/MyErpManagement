namespace MyErpManagement.Api.Constants
{
    public class ResponseTextConstant
    {
        public static class Ok
        {
            public const string SuccessSendEmailCode = "驗證碼已發送";
        }
        public static class BadRequest
        {
            public const string RequiredQueryAccessToken = "沒有Query access_token參數";
            public const string InvalidAccount = "無效的帳號";
            public const string InvalidPassword = "無效的密碼";
            public const string InvalidEmailCode = "無效的Email驗證碼";
            public const string FailToSaveDb = "資料儲存失敗";
            public const string InvalidDto = "參數驗證失敗";

            public const string FailToCreateProduct = "商品新增失敗";
            public const string FailToUpdateProduct = "商品更新失敗";
            public const string FailToCreateProductCategory = "商品分類新增失敗";
            public const string FailToUpdateProductCategory = "商品分類修改失敗";

            public const string FailToUpdateCustomer = "客戶更新失敗";
            public const string FailToCreateCustomer = "客戶新增失敗";
            public const string FailToDeleteCustomer = "客戶移除失敗";
            public const string FailToCreateCustomerTag = "客戶標籤新增失敗";

            public const string FailToUpdateSupplier = "供應商更新失敗";
            public const string FailToCreateSupplier = "供應商新增失敗";
            public const string FailToDeleteSupplier = "供應商移除失敗";
            public const string FailToCreateSupplierTag = "供應商標籤新增失敗";

            public const string FailToSendEmailCode = "Email會員註冊驗證碼發送失敗";
            public const string FailToReSendEmailCode = "Email會員註冊驗證碼尚未失效";
            public const string FailToCreateUser = "使用者註冊失敗";

            public const string FailToCreatePurchaseOrder = "採購單新增失敗";
            public const string FailToUpdatePurchaseOrder = "採購單更新失敗";
            public const string FailToApprovePurchaseOrder = "採購單核准失敗";
            public const string FailToCancelPurchaseOrder = "採購單取消失敗";
            public const string FailToCompletePurchaseOrder = "採購單只有在狀態 取消或核准,才能改完成狀態";
            public const string FailToUpdatePurchaseOrderForNotDrift = "採購單只有狀態 草稿 才能修改資料";

            public const string FailToCreateTransferOrder = "調貨單新增失敗";
            public const string FailToUpdateTransferOrder = "調貨單更新失敗";
            public const string FailToApproveTransferOrder = "調貨單核准失敗";
            public const string FailToCancelTransferOrder = "調貨單取消失敗";
            public const string FailToCompleteTransferOrder = "調貨單只有在狀態 取消或核准,才能改完成狀態";
            public const string FailToUpdateTransferOrderForNotDrift = "調貨單只有狀態 草稿 才能修改資料";

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
            public const string CustomerTag = "客戶標籤不存在";
            public const string Supplier = "供應商不存在";
            public const string SupplierTag = "供應商標籤不存在";
            public const string Product = "商品不存在";
            public const string ProductCategory = "商品分類不存在";
            public const string PurchaseOrder = "採購單不存在";
            public const string Transfer = "調貨單不存在";
            public const string Inventory = "庫存不存在";
        }

        public static class Conflict
        {
            public const string DbEntityUniqueField = "資料重複：唯一索引限制。";
            public const string EmailAlreadyInUse = "Email已被使用";
        }
    }
}
