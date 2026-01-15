using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Filters
{
    /// <summary>
    /// 此為Swagger生成api json文件用,
    /// 把propertyName* 的field 從Api ResponseDto中強制移除
    /// </summary>
    public class StrictPropertiesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // 避免ResponseDto回傳會多"propertyName*": "anything"
            schema.AdditionalPropertiesAllowed = true;
        }
    }
}
