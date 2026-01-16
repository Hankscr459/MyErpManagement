using System.Text.Json.Serialization;

namespace MyErpManagement.Core.Modules.ProductsModule.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // 讓 Swagger/API 顯示字串而非數字
    public enum ProductListSortByEnum
    {
        createdAt,
        name,
        salesPrice,
        purchasePrice,
    }
}
