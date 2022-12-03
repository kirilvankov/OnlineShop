namespace OnlineShop.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Payment;
    using static OnlineShop.Data.DataConstants;

    public class PaymentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TransactionMaxLength)]
        public string TransactionId { get; set; }

        [Required]
        [MaxLength(StatusMaxLength)]
        public string Status { get; set; }

        [Required]
        [MaxLength(CurrencyCodeMaxLength)]
        public string CurrencyCode { get; set; }

        public decimal CurrencyValue { get; set; }

        public DateTime CreatedOn { get; set; }

        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual OrderEntity Order { get; set; }
    }
}
