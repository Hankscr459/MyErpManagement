using System.Text.RegularExpressions;

namespace MyErpManagement.Api.Conventions
{
    public class KebabCaseParameterTransformer : IOutboundParameterTransformer
    {
        /// <summary>
        /// 把Controller的Route改成 kebab-case 轉換 ex: PurchaseOrder => purchase-order
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string? TransformOutbound(object? value)
        {
            if (value == null) return null;

            return Regex.Replace(
                value.ToString()!,
                "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100))
                .ToLowerInvariant();
        }
    }
}
