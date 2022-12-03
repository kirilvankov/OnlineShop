namespace OnlineShop.Data.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;
    using static OnlineShop.Data.DataConstants;

    public class OrderEntity
    {
        public OrderEntity()
        {
            Products = new HashSet<OrderProductEntity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual PaymentEntity Payment { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderProductEntity> Products { get; set; }
        public virtual ICollection<OrderAddressEntity> Addresses { get; set; }
    }
}
