using MyErpManagement.Core.Modules.PurchaseOrderModule.Models;

namespace MyErpManagement.Core.Modules.PurchaseOrderModule.IServices
{
    public interface IPurchaseOrderService
    {
        public Decimal CountTotalPrice(IEnumerable<PurchaseOrderCountTotalPriceModel> purchaseOrderLines);
    }
}
