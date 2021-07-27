namespace Exam_Project.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataConstants
    {
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
