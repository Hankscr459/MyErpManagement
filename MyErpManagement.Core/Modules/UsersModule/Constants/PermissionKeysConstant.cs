namespace MyErpManagement.Core.Modules.UsersModule.Constants
{
    public static class PermissionKeysConstant
    {
        public static class Auth
        {
        }

        public static class Product
        {
            public static class CreateProduct
            {
                public const string Key = "Product:CreateProduct";
                public const string Name = "Product:建立商品";
            }

            public static class CreateProductCategory
            {
                public const string Key = "Product:CreateProductCategory";
                public const string Name = "Product:建立商品分類";
            }

            public static class ReadProduct
            {
                public const string Key = "Product:ReadProduct";
                public const string Name = "Product:查看商品";
            }

            public static class ReadProductList
            {
                public const string Key = "Product:ReadProductList";
                public const string Name = "Product:查看商品清單";
            }

            public static class UpdateProduct
            {
                public const string Key = "Product:UpdateProduct";
                public const string Name = "Product:修改商品";
            }

        }

        public static class ProductCategory
        {
            public static class ReadProductCategory
            {
                public const string Key = "ProductCategory:ReadProductCategory";
                public const string Name = "ProductCategory:查看商品分類";
            }

            public static class UpdateProductCategory
            {
                public const string Key = "ProductCategory:UpdateProductCategory";
                public const string Name = "ProductCategory:修改商品分類";
            }

            public static class ReadProductCategoryTree
            {
                public const string Key = "ProductCategory:ReadProductCategoryTree";
                public const string Name = "ProductCategory:查看商品分類樹狀結構";
            }

            public static class ReadProductCategories
            {
                public const string Key = "ProductCategory:ReadProductCategories";
                public const string Name = "ProductCategory:查看商品分類清單";
            }
        }

        public static class Customer
        {
            public static class CreateCustomer
            {
                public const string Key = "Customer:CreateCustomer";
                public const string Name = "Customer:建立客戶";
            }

            public static class ReadCustomer
            {
                public const string Key = "Customer:ReadCustomer";
                public const string Name = "Customer:查看客戶";
            }

            public static class ReadCustomerList
            {
                public const string Key = "Customer:ReadCustomerList";
                public const string Name = "Customer:查看客戶清單";
            }

            public static class UpdateCustomer
            {
                public const string Key = "Customer:UpdateCustomer";
                public const string Name = "Customer:修改客戶";
            }

            public static class DeleteCustomer
            {
                public const string Key = "Customer:DeleteCustomer";
                public const string Name = "Customer:移除單筆客戶";
            }

            public static class DeleteManyCustomers
            {
                public const string Key = "Customer:DeleteManyCustomers";
                public const string Name = "Customer:移除多筆客戶";
            }
        }

        public static class CustomerTag
        {
            public static class CreateCustomerTag
            {
                public const string Key = "CustomerTag:CreateCustomerTag";
                public const string Name = "CustomerTag:建立客戶標籤";
            }

            public static class ReadCustomerTag
            {
                public const string Key = "CustomerTag:ReadCustomerTag";
                public const string Name = "CustomerTag:查看客戶標籤";
            }

            public static class ReadCustomerTags
            {
                public const string Key = "CustomerTag:ReadCustomerTags";
                public const string Name = "CustomerTag:查看客戶標籤清單";
            }

            public static class UpdateCustomerTag
            {
                public const string Key = "CustomerTag:UpdateCustomerTag";
                public const string Name = "CustomerTag:修改客戶標籤";
            }
        }

        public static class Supplier
        {
            public static class CreateSupplier
            {
                public const string Key = "Supplier:CreateSupplier";
                public const string Name = "Supplier:建立供應商";
            }

            public static class ReadSupplier
            {
                public const string Key = "Supplier:ReadSupplier";
                public const string Name = "Supplier:查看供應商";
            }

            public static class ReadSupplierList
            {
                public const string Key = "Supplier:ReadSupplierList";
                public const string Name = "Supplier:查看供應商清單";
            }

            public static class UpdateSupplier
            {
                public const string Key = "Supplier:UpdateSupplier";
                public const string Name = "Supplier:修改供應商";
            }

            public static class DeleteSupplier
            {
                public const string Key = "Supplier:DeleteSupplier";
                public const string Name = "Supplier:移除單筆供應商";
            }

            public static class DeleteManySuppliers
            {
                public const string Key = "Supplier:DeleteManySuppliers";
                public const string Name = "Supplier:移除多筆供應商";
            }
        }

        public static class SupplierTag
        {
            public static class CreateSupplierTag
            {
                public const string Key = "SupplierTag:CreateSupplierTag";
                public const string Name = "SupplierTag:建立客戶標籤";
            }

            public static class ReadSupplierTag
            {
                public const string Key = "SupplierTag:ReadSupplierTag";
                public const string Name = "SupplierTag:查看客戶標籤";
            }

            public static class ReadSupplierTags
            {
                public const string Key = "SupplierTag:ReadSupplierTags";
                public const string Name = "SupplierTag:查看客戶標籤清單";
            }

            public static class UpdateSupplierTag
            {
                public const string Key = "SupplierTag:UpdateSupplierTag";
                public const string Name = "SupplierTag:修改客戶標籤";
            }
        }

        public static class PurchaseOrder
        {
            public static class CreatePurchaseOrder
            {
                public const string Key = "PurchaseOrder:CreatePurchaseOrder";
                public const string Name = "PurchaseOrder:新增採購單";
            }

            public static class UpdatePurchaseOrder
            {
                public const string Key = "PurchaseOrder:UpdatePurchaseOrder";
                public const string Name = "PurchaseOrder:修改採購單";
            }

            public static class ApprovePurchaseOrder
            {
                public const string Key = "PurchaseOrder:ApprovePurchaseOrder";
                public const string Name = "PurchaseOrder:核准採購單";
            }

            public static class CancelPurchaseOrder
            {
                public const string Key = "PurchaseOrder:CancelPurchaseOrder";
                public const string Name = "PurchaseOrder:取消採購單";
            }

            public static class CompletePurchaseOrder
            {
                public const string Key = "PurchaseOrder:CompletePurchaseOrder";
                public const string Name = "PurchaseOrder:完成採購單";
            }
        }
    }
}
