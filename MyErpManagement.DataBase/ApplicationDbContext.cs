using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.JwtModule.Entities;
using MyErpManagement.Core.Modules.OrderNoModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Entities;
using MyErpManagement.Core.Modules.SupplierModule.Entities;
using MyErpManagement.Core.Modules.TransferOrderModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.DataBase.Configurations;
using MyErpManagement.DataBase.Configurations.CustomerConfigurations;
using MyErpManagement.DataBase.Configurations.InventoryConfiguraions;
using MyErpManagement.DataBase.Configurations.ProductConfigurations;
using MyErpManagement.DataBase.Configurations.PurchaseOrderConfigurations;
using MyErpManagement.DataBase.Configurations.SupplierConfigurations;
using MyErpManagement.DataBase.Configurations.TransferOrderConfigurations;
using MyErpManagement.DataBase.Configurations.UserConfiguraions;

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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerTag> CustomerTags { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierTag> SupplierTags { get; set; }
        public DbSet<OrderSequence> OrderSequences { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryPolicy> InventoryPolicies { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<TransferOrder> TransferOrders { get; set; }
        public DbSet<TransferOrderLine> TransferOrderLines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration(_config));
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new JwtRecordConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerTagConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerTagRelationConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierTagConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierTagRelationConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new OrderSequenceConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguraion());
            modelBuilder.ApplyConfiguration(new InventoryTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryPolicyConfiguration());
            modelBuilder.ApplyConfiguration(new WareHouseConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderLineConfiguration());
            modelBuilder.ApplyConfiguration(new TransferOrderConfiguration());
            modelBuilder.ApplyConfiguration(new TransferOrderLineConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
