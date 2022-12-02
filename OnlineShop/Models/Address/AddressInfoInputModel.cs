namespace OnlineShop.Models.Address
{
    using System.ComponentModel.DataAnnotations;

    public class AddressInfoInputModel
    {
        [Required]
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string PostCode { get; set; }

        [RegularExpression("^(\\+|-)?(?:90(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\\.[0-9]{1,6})?))$", ErrorMessage = "Latitude is invaluid")]
        public double? LocationLat { get; set; }

        [RegularExpression("^(\\+|-)?(?:180(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\\.[0-9]{1,6})?))$", ErrorMessage = "Longitude is invalid")]
        public double? LocationLng { get; set; }

        public bool IsUserAddress { get; set; }
    }
}
