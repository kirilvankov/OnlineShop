namespace OnlineShop.Services.Models
{
    using OnlineShop.Data.Enums;

    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalDetails { get; set; }
        public string UserId { get; set; }
        public Status Status { get; set; }
        public AddressInfoDto AddressInfo { get; set; }
    }
}
