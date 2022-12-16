namespace OnlineShop.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OnlineShop.Data.Enums;

    using static DataConstants.Store;
    public class StoreEntity
    {
        public StoreEntity()
        {
            Products = new HashSet<ProductEntity>();
        }
        [Key]
        public int Id { get; set; }

        [Required, StringLength(StoreNameMaxLength, MinimumLength = StoreNameMinLength)]
        public string Name { get; set; }

        [Required,StringLength(StoreDescriptionMaxLength, MinimumLength = StoreDescriptionMinLength)]
        public string Description { get; set; }

        [Required, MaxLength(StoreAdditionalInfoMaxLength)]
        public string AdditionalInfo { get; set; }

        public Status Status { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual AddressInfoEntity AddressInfo { get; set; }

        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
