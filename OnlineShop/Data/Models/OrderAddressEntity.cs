namespace OnlineShop.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class OrderAddressEntity
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public int AddressInfoId { get; set; }
        public AddressInfoEntity AddressInfo { get; set; }
    }
}
