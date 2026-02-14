using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories.InventoryRepositories
{
    public class InventoryPolicyRepository : Repository<InventoryPolicy>, IInventoryPolicyRepository
    {
        public InventoryPolicyRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
