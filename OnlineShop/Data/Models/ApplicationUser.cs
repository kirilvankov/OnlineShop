namespace OnlineShop.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new HashSet<OrderEntity>();
            Addresses = new HashSet<AddressInfoEntity>();
        }
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }

        public virtual ICollection<AddressInfoEntity> Addresses { get; set; }
    }
}
