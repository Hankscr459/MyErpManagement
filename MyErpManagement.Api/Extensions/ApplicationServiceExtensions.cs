using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyErpManagement.Api.Examples.Auth;
using MyErpManagement.Api.Filters;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Exceptions.IParsers;
using MyErpManagement.Core.Exceptions.Parsers;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Services;
using MyErpManagement.Core.Modules.ProductsModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using MyErpManagement.Core.Modules.UsersModule.Services;
using MyErpManagement.DataBase;
using MyErpManagement.DataBase.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace MyErpManagement.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config, IWebHostEnvironment env)
        {
            services.AddControllers(options =>
            {
                // 移除其他 Formatter 或確保 JsonFormatter 在首位
                options.ReturnHttpNotAcceptable = true;
            })
                .ConfigureApiBehaviorOptions(opt =>
                {
                    // 當 ModelState 無效時，執行此處理邏輯
                    opt.InvalidModelStateResponseFactory = ValidationResultHelper.HandleInvalidModelState;
                });
            services.AddSwaggerGen(c =>
            {
                // 1. 取得 Api 專案自身的 XML (包含 Controller 的註解)
                var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                if (File.Exists(apiXmlPath)) c.IncludeXmlComments(apiXmlPath);

                // 2. 取得 Core 專案的 XML (包含 DTOs 的註解)
                // 這裡的名稱必須與你的 MyErpManagement.Core 專案名稱一致
                var coreXmlFile = "MyErpManagement.Core.xml";
                var coreXmlPath = Path.Combine(AppContext.BaseDirectory, coreXmlFile);
                if (File.Exists(coreXmlPath)) c.IncludeXmlComments(coreXmlPath);

                // http://localhost:5228/swagger/v1/swagger.json
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT token must be provided",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.SchemaFilter<StrictPropertiesSchemaFilter>();
                // 必須加上這一行，才會去讀取 [SwaggerResponseExample] 屬性
                c.ExampleFilters();
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            services.AddSwaggerExamplesFromAssemblyOf<BadRequestLoginResponseExample>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config["Sql_Server_Connection_String"]));
            services.AddCors();
            services.AddScoped<IExceptionParser, SqlExceptionParser>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPermissionSyncService, PermissionSyncService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IJwtRecordRepository, JwtRecordRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerTagRepository, CustomerTagRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMapster();
            return services;
        }
    }
}
