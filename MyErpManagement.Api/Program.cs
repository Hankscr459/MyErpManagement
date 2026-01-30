using IGeekFan.AspNetCore.RapiDoc;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Extensions;
using MyErpManagement.Api.Middleware;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using MyErpManagement.DataBase;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddCoreValidators();


var app = builder.Build();

// 設定 Middleware 順序 (Pipeline)
app.UseRouting();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    // 檢查是否有待處理的 Migration，並應用它們
    // 這會創建資料庫和表格，並執行 SeedData 邏輯
    if (context.Database.IsRelational())
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        await context.Database.EnsureCreatedAsync();
    }
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

var syncService = services.GetRequiredService<IPermissionSyncService>();
await syncService.SyncAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // 配置 Scalar UI
    // http://localhost:5228/scalar/v1
    //app.MapScalarApiReference(options =>
    //{
    //    options.OpenApiRoutePattern = "/swagger/v1/swagger.json";
    //    options
    //        .WithTitle("API 文件") // 瀏覽器分頁標題
    //        .WithTheme(ScalarTheme.Moon)   // 設定主題 (Moon, Solarized, BluePlanet 等)
    //        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.HttpClient); // 預設產出的程式碼範例
    //});

    // 3. 配置 RapiDoc http://localhost:5228/index.html#overview
    app.UseRapiDocUI(c =>
    {
        c.RoutePrefix = ""; // serve the UI at root // http://localhost:5228/v1/api-docs
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
        //https://mrin9.github.io/RapiDoc/api.html
        //This Config Higher priority
        c.GenericRapiConfig = new GenericRapiConfig()
        {
            RenderStyle = "read",
            Theme = "light",//light | dark
            SchemaStyle = "table"////tree | table
        };
    });
}


app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// 先執行標準驗證 (檢查 JWT 是否合法/格式正確) .net 10 預設拿掉 
app.UseAuthentication();

// 最後檢查權限 (是否有 Admin 角色等)
app.UseAuthorization();

// 啟用 Controller 路由映射，讓應用程式知道如何處理您的 AuthController
app.MapControllers();

app.MapGet("/", () => new { root = "/" });
app.Run();

//在 .NET 10 的 API 專案中，預設使用 Top-level statements（沒有顯式的 class Program）。
//為了讓測試專案 MyErpManagement.IntegrationTests 能夠引用 Program 類別來啟動伺服器
public partial class Program { }
