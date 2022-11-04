namespace OnlineShop.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AddressInfoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string PostCode { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLng { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int StoreId { get; set; }

        [ForeignKey(nameof(StoreId))]
        public virtual StoreEntity Store { get; set; }

        public virtual ICollection<OrderAddressEntity> Orders { get; set; }
    }
}
