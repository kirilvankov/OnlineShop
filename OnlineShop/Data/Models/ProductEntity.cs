namespace OnlineShop.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Product;

    public class ProductEntity
    {
        public ProductEntity()
        {
            Orders = new HashSet<OrderProductEntity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual CategoryEntity Category { get; set; }

        public int? StoreId { get; set; }

        public virtual StoreEntity Store { get; set; }

        public virtual ICollection<OrderProductEntity> Orders { get; set; }
    }
}
