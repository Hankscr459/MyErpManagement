using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
