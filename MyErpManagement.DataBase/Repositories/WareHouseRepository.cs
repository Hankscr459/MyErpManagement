using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class WareHouseRepository : Repository<WareHouse>, IWareHouseRepository
    {
        public WareHouseRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
