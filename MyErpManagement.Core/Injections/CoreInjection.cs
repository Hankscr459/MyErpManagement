using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Core.Exceptions.IParsers;
using MyErpManagement.Core.Exceptions.Parsers;
using MyErpManagement.Core.Modules.CacheModule.IServices;
using MyErpManagement.Core.Modules.CacheModule.Services;
using MyErpManagement.Core.Modules.EmailModule.IServices;
using MyErpManagement.Core.Modules.EmailModule.Services;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Services;
using MyErpManagement.Core.Modules.MessageBusModule.IServices;
using MyErpManagement.Core.Modules.MessageBusModule.Services;
using MyErpManagement.Core.Modules.OrderNoModule.IServices;
using MyErpManagement.Core.Modules.OrderNoModule.Services;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using MyErpManagement.Core.Modules.UsersModule.Services;

namespace MyErpManagement.Core.Injections
{
    public static class CoreInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                // 從設定檔讀取連接字串
                options.Configuration = config["Redis"];
                // 選填：為所有 Key 加上前綴，避免多個應用程式衝突
                options.InstanceName = config["Redis_Instance_Name"];
            });

            services.AddScoped<IPermissionSyncService, PermissionSyncService>();
            services.AddScoped<IExceptionParser, SqlExceptionParser>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICachService, CachService>();
            services.AddScoped<IOrderNoService, OrderNoService>();
            // 註冊 RabbitMQ 的發送者 (讓 API 控制器使用)
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }
}
