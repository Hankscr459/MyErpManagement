using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;
using MyErpManagement.DataBase.Repositories;

namespace MyErpManagement.DataBase.Injections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config)
        {
            // 1. 註冊 DbContext (原本在 API 寫的搬過來)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(config["Postgres_Sql_Connection_String"]));

            // 2. 註冊所有的 Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IJwtRecordRepository, JwtRecordRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerTagRepository, CustomerTagRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
