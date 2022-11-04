namespace OnlineShop.Models.Store
{
    using OnlineShop.Data.Enums;

    public class StoreViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public Status Status { get; set; }
    }
}
