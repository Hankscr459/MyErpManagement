using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.CustomerModule.IRepositories;

namespace MyErpManagement.DataBase.Repositories.CustomerRepositories
{
    public class CustomerTagRepository : Repository<CustomerTag>, ICustomerTagRepository
    {
        public CustomerTagRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
