using MyErpManagement.Core.Modules.SupplierModule.Entities;
using MyErpManagement.Core.Modules.SupplierModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories.SupplierRepositories
{
    public class SupplierTagRepository : Repository<SupplierTag>, ISupplierTagRepository
    {
        public SupplierTagRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
