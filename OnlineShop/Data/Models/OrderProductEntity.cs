namespace OnlineShop.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.OrderProduct;

    public class OrderProductEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        [Range(QuantityMinValue, QuantityMaxValue)]
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual ProductEntity Product { get; set; }

        public int OrderId { get; set; }

        [Required]
        public virtual OrderEntity Order { get; set; }
    }
}
