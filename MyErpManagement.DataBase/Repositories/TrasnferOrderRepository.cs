using MyErpManagement.Core.Modules.TransferOrderModule.Entities;
using MyErpManagement.Core.Modules.TransferOrderModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class TrasnferOrderRepository : Repository<TransferOrder>, ITransferOrderRepository
    {
        public TrasnferOrderRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
