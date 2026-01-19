namespace MyErpManagement.Core.Modules.ProductsModule.Models
{
    public class ProductCategoryTreeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public int SortOrder { get; set; }
        public List<ProductCategoryTreeModel> Children { get; set; } = new List<ProductCategoryTreeModel>();
    }
}
