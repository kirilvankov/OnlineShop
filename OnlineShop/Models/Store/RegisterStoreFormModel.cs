namespace OnlineShop.Models.Store
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterStoreFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string AdditionalDetails { get; set; }
        public AddressInfoViewModel AddressInfo { get; set; }
    }
}
