using System.Text.Json.Serialization;

namespace MyErpManagement.Core.Modules.CustomerModule.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CustomerListSortByEnum
    {
        name,
        code,
        createdAt,
        updatedAt,
    }
}
