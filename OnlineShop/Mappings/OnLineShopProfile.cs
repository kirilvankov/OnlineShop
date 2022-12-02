namespace OnlineShop.Mappings
{
    using AutoMapper;

    using OnlineShop.Models.Address;

    public class OnLineShopProfile : Profile
    {
        public OnLineShopProfile()
        {
            CreateMap<AddressInfoViewModel, AddressInfoInputModel>();
        }
    }
}
