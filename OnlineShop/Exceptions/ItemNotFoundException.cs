namespace OnlineShop.Exceptions
{
    using System;

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
