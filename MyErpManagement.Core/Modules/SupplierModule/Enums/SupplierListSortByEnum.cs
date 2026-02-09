using System.Text.Json.Serialization;

namespace MyErpManagement.Core.Modules.SupplierModule.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SupplierListSortByEnum
    {
        name,
        code,
        createdAt,
        updatedAt,
    }
}
