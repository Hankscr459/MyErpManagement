namespace MyErpManagement.Core.Modules.OrderNoModule.Entities
{
    public class OrderSequence
    {
        public string OrderType { get; set; } = null!;

        public string Period { get; set; } = null!; // YYYYMM

        public int CurrentNo { get; set; }
    }
}
