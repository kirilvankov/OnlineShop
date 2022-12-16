namespace OnlineShop.Services.Models
{
    public class OrderUserDetailsDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressInfoDto AddressInfo { get; set; }
    }
}
