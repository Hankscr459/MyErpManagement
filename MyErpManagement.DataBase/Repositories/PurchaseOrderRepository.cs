using MyErpManagement.Core.Modules.PurchaseOrderModule.Entities;
using MyErpManagement.Core.Modules.PurchaseOrderModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
