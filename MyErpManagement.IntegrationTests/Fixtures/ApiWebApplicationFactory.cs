using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.DataBase;

namespace MyErpManagement.IntegrationTests.Fixtures
{
    // 繼承 WebApplicationFactory 並指定 API 的入口點 Program
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. 移除主專案中可能已經註冊的 DbContext
                var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));
                if (descriptor != null) services.Remove(descriptor);


                // 2. 注入 InMemory 
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestDb");
                });

                // 3. 初始化 Seed Data (維持你原本的邏輯)
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //db.Database.EnsureDeleted(); // 確保每次測試開始前是乾淨的
                db.Database.EnsureCreated();
            });
        }
    }
}
