using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Models;

namespace MyErpManagement.Core.Modules.CustomerModule.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IQueryable<Customer> GetSearchQuery(CustomerListQueryModel queryModel);
    }
}
