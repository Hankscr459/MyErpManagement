using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.JwtModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using MyErpManagement.DataBase.Configurations;

namespace MyErpManagement.DataBase
{
    public class ApplicationDbContext(DbContextOptions options, IConfiguration config) : DbContext(options)
    {
        private readonly IConfiguration _config = config;

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<JwtRecord> JwtRecords { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration(_config));
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new JwtRecordConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            // 2. 設定 UserRole 中間表與複合主鍵
            modelBuilder.Entity<UserRole>(builder =>
            {
                builder.HasKey(ur => new { ur.UserId, ur.RoleId });

                // 設定一對多關係組成多對多
                builder.HasOne(ur => ur.User)
                       .WithMany(u => u.UserRoles)
                       .HasForeignKey(ur => ur.UserId);

                builder.HasOne(ur => ur.Role)
                       .WithMany(r => r.UserRoles)
                       .HasForeignKey(ur => ur.RoleId);
            });

            // 3. 設定 RolePermission 中間表與複合主鍵
            modelBuilder.Entity<RolePermission>(builder =>
            {
                builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                builder.HasOne(rp => rp.Role)
                       .WithMany(r => r.RolePermissions)
                       .HasForeignKey(rp => rp.RoleId);

                builder.HasOne(rp => rp.Permission)
                       .WithMany(p => p.RolePermissions)
                       .HasForeignKey(rp => rp.PermissionId);
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
