using MyErpManagement.Core.Modules.OrderNoModule.Entities;
using MyErpManagement.Core.Modules.OrderNoModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class OrderSequenceRepository : Repository<OrderSequence>, IOrderSequenceRepository
    {
        public OrderSequenceRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
