namespace OnlineShop.Services.Models
{
    public class AddressInfoDto
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PostCode { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLng { get; set; }
        public bool IsPrimary { get; set; }
    }
}