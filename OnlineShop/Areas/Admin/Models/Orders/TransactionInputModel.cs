namespace OnlineShop.Areas.Admin.Models.Orders
{
    using System;  
    

    public class TransactionInputModel
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public AmountInputModel Amount { get; set; }
        public DateTime create_time { get; set; }
    }
    public class AmountInputModel
    {
        public string currency_code { get; set; }
        public decimal Value { get; set; }
    }
}
