namespace Exam_Project.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataConstants
    {
        public class User
        {
            public const int UserMinLength = 2;
            public const int FirstNameMaxLength = 25;
            public const int LastNameMaxLength = 30;
            public const int AddressMaxLength = 100;
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
        public class OrderItem
        {
            public const int NameMaxLength = 50;
        }
    }
}
