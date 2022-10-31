namespace OnlineShop.Services.Models
{
    public class RegisterStoreDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalDetails { get; set; }
        public AddressInfoDto AddressInfo { get; set; }
    }
}
