namespace OnlineShop.Mappings
{
    using AutoMapper;

    using OnlineShop.Models.Address;
    using OnlineShop.Services.Models;

    public class OnLineShopProfile : Profile
    {
        public OnLineShopProfile()
        {
            CreateMap<AddressInfoDto, AddressInfoInputModel>()
                .ReverseMap();
            CreateMap<AddressInfoDto, AddressInfoViewModel>()
                .ReverseMap();
        }
    }
}
