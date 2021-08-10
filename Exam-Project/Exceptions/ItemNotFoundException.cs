namespace Exam_Project.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException()
        {

        }
        public ItemNotFoundException(string message)
                : base(message)
        {

        }
    }
}
