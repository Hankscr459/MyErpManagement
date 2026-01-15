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

            public static class ReadProductList
            {
                public const string Key = "Product:ReadProductList";
                public const string Name = "Product:查看商品清單";
            }
        }
    }
}
