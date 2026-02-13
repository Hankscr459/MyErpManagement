using MyErpManagement.Core.Modules.PurchaseOrderModule.IServices;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Models;

namespace MyErpManagement.Core.Modules.PurchaseOrderModule.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        public decimal CountTotalPrice(IEnumerable<PurchaseOrderCountTotalPriceModel> purchaseOrderLines)
        {
            Decimal totalPrice = 0;
            foreach (var item in purchaseOrderLines)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
