namespace MyErpManagement.Core.Modules.UsersModule.Constants
{
    public static class PermissionKeysConstant
    {
        public static class Auth
        {
            public static class Secret
            {
                public const string Key = "Auth:Secret";
                public const string Name = "Auth:權限測試";
            };
            public static class Secret1
            {
                public const string Key = "Auth:Secret1";
                public const string Name = "Auth:權限測試1";
            };
            public static class Secret2
            {
                public const string Key = "Auth:Secret2";
                public const string Name = "Auth:權限測試2";
            };
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
    }
}
