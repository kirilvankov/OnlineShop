﻿namespace OnlineShop.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Services.Models;

    public interface IUserService
    {
        Task<OrderUserDetailsDto> GetUserDetails(string userId, CancellationToken cancellationToken);

        void SetUserPrimaryAddress(AddressInfoDto address, string userId);
    }
}
