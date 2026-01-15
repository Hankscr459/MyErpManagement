namespace MyErpManagement.Core.Validators.Constants
{
    public static class VaildatorConstant
    {
        public const string Required = "為必填欄位";

        // 注意：{MinLength} 是 FluentValidation 識別的關鍵字
        public const string MinLengthFormat = "長度至少需要 {MinLength} 位";
    }
}
