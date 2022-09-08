namespace OnlineShop.Data.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.OrderItem;

    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } 

        [Required]
        [MaxLength(NameMaxLength)]
        public string ProductName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int OrderId { get; set; }

        [Required]
        public Order Order { get; set; }


    }
}
