using System.Text.Json.Serialization;

namespace MyErpManagement.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirEnum
    {
        asc,
        desc,
    }
}
