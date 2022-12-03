namespace OnlineShop.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataConstants
    {
        public class User
        {
            public const int UserNameMinLength = 2;
            public const int FirstNameMaxLength = 25;
            public const int LastNameMaxLength = 30;
        }
        public class Product
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;

            public const int DescriptionMinLength = 2;
            public const int DescriptionMaxLength = 3000;
        }
        public class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }
        public class OrderProduct
        {
            public const int QuantityMinValue = 1;
            public const int QuantityMaxValue = int.MaxValue;
        }
        public class Store
        {
            public const int StoreNameMinLength = 2;
            public const int StoreNameMaxLength = 50;

            public const int StoreDescriptionMinLength = 2;
            public const int StoreDescriptionMaxLength = 3000;

            public const int StoreAdditionalInfoMaxLength = 3000;
        }
        public class Payment
        {
            public const int TransactionMaxLength = 50;

            public const int StatusMaxLength = 25;

            public const int CurrencyCodeMaxLength = 15;
        }
    }
}
