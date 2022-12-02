namespace OnlineShop.Models.Address
{
    public class AddressInfoViewModel
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string PostCode { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLng { get; set; }
    }
}
