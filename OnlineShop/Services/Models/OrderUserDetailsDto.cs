namespace OnlineShop.Services.Models
{
    using System.Data.Common;

    public class OrderUserDetailsDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressInfoDto AddressInfo { get; set; }
    }
}
