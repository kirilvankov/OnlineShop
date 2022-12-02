namespace OnlineShop.Models.Store
{
    using System.ComponentModel.DataAnnotations;

    using OnlineShop.Models.Address;

    public class RegisterStoreFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string AdditionalDetails { get; set; }
        public AddressInfoInputModel AddressInfo { get; set; }
    }
}
