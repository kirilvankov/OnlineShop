namespace OnlineShop.Services
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Services.Models;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OrderUserDetailsDto> GetUserDetails(string userId, CancellationToken cancellationToken)
        {
           return await _dbContext.Users.Where(x => x.Id == userId).Select(x => new OrderUserDetailsDto()
           {
               Id = x.Id,
               FirstName = x.FirstName,
               LastName = x.LastName,
               AddressInfo = x.Addresses.Where(x => x.IsPrimary == true).Select(x => new AddressInfoDto()
               {
                   AddressLine1 = x.AddressLine1,
                   AddressLine2 = x.AddressLine2,
                   PhoneNumber = x.PhoneNumber,
                   PostCode = x.PostCode,
                   Email = x.Email,
                   City = x.City,
                   LocationLat = x.LocationLat,
                   LocationLng = x.LocationLng,
               }).FirstOrDefault(),
           }).FirstOrDefaultAsync(cancellationToken);
        }

        public void SetUserPrimaryAddress(AddressInfoDto address, string userId)
        {
            var addressUser = _dbContext.AddressInfo.Where(x => x.User.Id == userId && x.IsPrimary == true).FirstOrDefault();
            var addressinfo = new AddressInfoEntity()
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PhoneNumber = address.PhoneNumber,
                City = address.City,
                PostCode = address.PostCode,
                Email = address.Email,
                UserId = userId,
                IsPrimary= address.IsPrimary,
                
            };

            if (address.IsPrimary)
            {
                if (addressUser == null)
                {
                    addressUser = addressinfo;
                    _dbContext.Add(addressinfo);
                }
                else
                {
                    addressUser.AddressLine1 = address.AddressLine1;
                    addressUser.AddressLine2 = address.AddressLine2;
                    addressUser.PhoneNumber = address.PhoneNumber;
                    addressUser.City = address.City;
                    addressUser.PostCode = address.PostCode;
                    addressUser.Email = address.Email;
                    addressUser.IsPrimary= address.IsPrimary;
                }
            }
            
            _dbContext.SaveChanges();
        }

    }
}
